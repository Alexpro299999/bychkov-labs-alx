using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Data;
using RealEstateAgency.DataAccess.Repositories;
using RealEstateAgency.DataAccess.Models;
using RealEstateAgency.WPF.Services;
using System.Collections.Generic;

namespace RealEstateAgency.WPF.Views
{
    public partial class MainWindow : Window
    {
        private EmployeeRepository _empRepo = new EmployeeRepository();
        private ServiceRepository _srvRepo = new ServiceRepository();
        private ContractRepository _cntRepo = new ContractRepository();
        private WordReportGenerator _reportGen = new WordReportGenerator();

        private List<Employee> _allEmployees = new List<Employee>();

        public MainWindow()
        {
            InitializeComponent();
            LoadData();
            LoadQueryCombo();
        }

        private void LoadData()
        {
            _allEmployees = _empRepo.GetAll();
            GridEmployees.ItemsSource = _allEmployees;
            GridServices.ItemsSource = _srvRepo.GetAll();
            GridContracts.ItemsSource = _cntRepo.GetAll();
        }

        private void LoadQueryCombo()
        {
            CmbQueryEmployee.ItemsSource = _allEmployees;
        }


        private void CmbQueries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CmbQueries.SelectedIndex == 2)
            {
                PanelParams.Visibility = Visibility.Visible;
            }
            else
            {
                PanelParams.Visibility = Visibility.Collapsed;
            }
            GridQueryResults.ItemsSource = null;
        }

        private void BtnExecuteQuery_Click(object sender, RoutedEventArgs e)
        {
            int index = CmbQueries.SelectedIndex;
            if (index == -1) return;

            try
            {
                switch (index)
                {
                    case 0: // Высшее образование
                        GridQueryResults.ItemsSource = _empRepo.GetWithHigherEducation();
                        break;
                    case 1: // Популярные услуги
                        GridQueryResults.ItemsSource = _srvRepo.GetPopularServices();
                        break;
                    case 2: // Договора сотрудника
                        if (CmbQueryEmployee.SelectedValue == null)
                        {
                            MessageBox.Show("Выберите сотрудника!");
                            return;
                        }
                        int empId = (int)CmbQueryEmployee.SelectedValue;
                        GridQueryResults.ItemsSource = _cntRepo.GetByEmployee(empId);
                        break;
                    case 3: // Статистика
                        GridQueryResults.ItemsSource = _cntRepo.GetEmployeeStats().DefaultView;
                        break;
                    case 4: // Группировка по датам
                        GridQueryResults.ItemsSource = _cntRepo.GetServiceDateStats().DefaultView;
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка выполнения: " + ex.Message);
            }
        }

        private void BtnExportQuery_Click(object sender, RoutedEventArgs e)
        {
            int index = CmbQueries.SelectedIndex;
            if (index == -1) return;

            try
            {
                switch (index)
                {
                    case 0:
                        // Пример отчета по списку
                        _reportGen.GenerateAllEmployeesReport((List<Employee>)GridQueryResults.ItemsSource);
                        break;
                    case 2:
                        // Отчет по сотруднику
                        if (CmbQueryEmployee.SelectedItem is Employee emp)
                        {
                            var contracts = (List<Contract>)GridQueryResults.ItemsSource;
                            _reportGen.GenerateEmployeeContractsReport(emp.FullName, contracts);
                        }
                        break;
                    case 3:
                        // Статистика
                        var dv = (DataView)GridQueryResults.ItemsSource;
                        if (dv != null) _reportGen.GenerateStatsReport(dv.Table);
                        break;
                    default:
                        MessageBox.Show("Для этого запроса экспорт в Word реализован в общем виде (таблица).");
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка экспорта: " + ex.Message);
            }
        }



        private void BtnAddEmployee_Click(object sender, RoutedEventArgs e)
        {
            var win = new AddEditEmployeeWindow();
            if (win.ShowDialog() == true) { _empRepo.Add(win.Employee); LoadData(); LoadQueryCombo(); }
        }

        private void BtnEditEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (GridEmployees.SelectedItem is Employee selected)
            {
                var win = new AddEditEmployeeWindow(selected);
                if (win.ShowDialog() == true) { _empRepo.Update(win.Employee); LoadData(); LoadQueryCombo(); }
            }
        }

        private void BtnDeleteEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (GridEmployees.SelectedItem is Employee selected && MessageBox.Show("Удалить?", "Confirm", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _empRepo.Delete(selected.Id); LoadData(); LoadQueryCombo();
            }
        }

        private void TxtSearchEmployee_TextChanged(object sender, TextChangedEventArgs e)
        {
            var filter = TxtSearchEmployee.Text.ToLower();
            GridEmployees.ItemsSource = _allEmployees.Where(x => x.FullName.ToLower().Contains(filter)).ToList();
        }

        private void BtnResetEmployeeFilter_Click(object sender, RoutedEventArgs e)
        {
            TxtSearchEmployee.Text = "";
            GridEmployees.ItemsSource = _allEmployees;
        }

        private void BtnAddService_Click(object sender, RoutedEventArgs e)
        {
            var win = new AddEditServiceWindow();
            if (win.ShowDialog() == true) { _srvRepo.Add(win.ServiceObj); LoadData(); }
        }

        private void BtnEditService_Click(object sender, RoutedEventArgs e)
        {
            if (GridServices.SelectedItem is Service selected)
            {
                var win = new AddEditServiceWindow(selected);
                if (win.ShowDialog() == true) { _srvRepo.Update(win.ServiceObj); LoadData(); }
            }
        }

        private void BtnDeleteService_Click(object sender, RoutedEventArgs e)
        {
            if (GridServices.SelectedItem is Service selected) { _srvRepo.Delete(selected.Id); LoadData(); }
        }

        private void BtnAddContract_Click(object sender, RoutedEventArgs e)
        {
            var win = new AddEditContractWindow();
            if (win.ShowDialog() == true) { _cntRepo.Add(win.ContractObj); LoadData(); }
        }

        private void BtnEditContract_Click(object sender, RoutedEventArgs e)
        {
            if (GridContracts.SelectedItem is Contract selected)
            {
                var win = new AddEditContractWindow(selected);
                if (win.ShowDialog() == true) { _cntRepo.Update(win.ContractObj); LoadData(); }
            }
        }

        private void BtnDeleteContract_Click(object sender, RoutedEventArgs e)
        {
            if (GridContracts.SelectedItem is Contract selected) { _cntRepo.Delete(selected.Id); LoadData(); }
        }

        private void BtnReportEmployees_Click(object sender, RoutedEventArgs e)
        {
            _reportGen.GenerateAllEmployeesReport(_allEmployees);
        }
    }
}
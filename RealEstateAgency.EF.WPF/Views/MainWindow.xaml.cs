using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using RealEstateAgency.EF.DataAccess.Data;
using RealEstateAgency.EF.DataAccess.Models;
using RealEstateAgency.EF.DataAccess.Repositories;
using RealEstateAgency.EF.WPF.Services;

namespace RealEstateAgency.EF.WPF.Views
{
    public partial class MainWindow : Window
    {
        private readonly RealEstateDbContext _context;
        private readonly EmployeeRepository _empRepo;
        private readonly ServiceRepository _srvRepo;
        private readonly ContractRepository _cntRepo;
        private readonly WordReportGenerator _reportGen;

        private List<Employee> _cachedEmployees;

        public MainWindow()
        {
            InitializeComponent();
            _context = new RealEstateDbContext();
            _empRepo = new EmployeeRepository(_context);
            _srvRepo = new ServiceRepository(_context);
            _cntRepo = new ContractRepository(_context);
            _reportGen = new WordReportGenerator();

            LoadAllData();
        }

        private void LoadAllData()
        {
            _cachedEmployees = _empRepo.GetAll();
            GridEmployees.ItemsSource = _cachedEmployees;
            GridServices.ItemsSource = _srvRepo.GetAll();
            GridContracts.ItemsSource = _cntRepo.GetAll();
            CmbQueryEmployee.ItemsSource = _cachedEmployees;
        }

        private void TxtSearchEmp_TextChanged(object sender, TextChangedEventArgs e)
        {
            var text = TxtSearchEmp.Text.ToLower();
            if (_cachedEmployees != null)
            {
                GridEmployees.ItemsSource = _cachedEmployees
                    .Where(x => x.FullName.ToLower().Contains(text))
                    .ToList();
            }
        }

        private void BtnResetEmp_Click(object sender, RoutedEventArgs e)
        {
            TxtSearchEmp.Text = "";
            GridEmployees.ItemsSource = _cachedEmployees;
        }

        private void BtnAddEmp_Click(object sender, RoutedEventArgs e)
        {
            var win = new AddEditEmployeeWindow();
            if (win.ShowDialog() == true)
            {
                _empRepo.Add(win.Employee);
                LoadAllData();
            }
        }

        private void BtnEditEmp_Click(object sender, RoutedEventArgs e)
        {
            if (GridEmployees.SelectedItem is Employee selected)
            {
                var win = new AddEditEmployeeWindow(selected);
                if (win.ShowDialog() == true)
                {
                    _empRepo.Update(win.Employee);
                    LoadAllData();
                }
            }
        }

        private void BtnDelEmp_Click(object sender, RoutedEventArgs e)
        {
            if (GridEmployees.SelectedItem is Employee selected)
            {
                if (MessageBox.Show($"Удалить {selected.FullName}?", "Confirm", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    _empRepo.Delete(selected.Id);
                    LoadAllData();
                }
            }
        }

        private void BtnReportAllEmps_Click(object sender, RoutedEventArgs e)
        {
            _reportGen.GenerateAllEmployeesReport(_cachedEmployees);
        }

        private void BtnAddService_Click(object sender, RoutedEventArgs e)
        {
            var win = new AddEditServiceWindow();
            if (win.ShowDialog() == true)
            {
                _srvRepo.Add(win.ServiceObj);
                LoadAllData();
            }
        }

        private void BtnEditService_Click(object sender, RoutedEventArgs e)
        {
            if (GridServices.SelectedItem is Service selected)
            {
                var win = new AddEditServiceWindow(selected);
                if (win.ShowDialog() == true)
                {
                    _srvRepo.Update(win.ServiceObj);
                    LoadAllData();
                }
            }
        }

        private void BtnDelService_Click(object sender, RoutedEventArgs e)
        {
            if (GridServices.SelectedItem is Service selected)
            {
                _srvRepo.Delete(selected.Id);
                LoadAllData();
            }
        }

        private void BtnAddContract_Click(object sender, RoutedEventArgs e)
        {
            var win = new AddEditContractWindow(_context);
            if (win.ShowDialog() == true)
            {
                _cntRepo.Add(win.ContractObj);
                LoadAllData();
            }
        }

        private void BtnEditContract_Click(object sender, RoutedEventArgs e)
        {
            if (GridContracts.SelectedItem is Contract selected)
            {
                var win = new AddEditContractWindow(_context, selected);
                if (win.ShowDialog() == true)
                {
                    _cntRepo.Update(win.ContractObj);
                    LoadAllData();
                }
            }
        }

        private void BtnDelContract_Click(object sender, RoutedEventArgs e)
        {
            if (GridContracts.SelectedItem is Contract selected)
            {
                _cntRepo.Delete(selected.Id);
                LoadAllData();
            }
        }

        private void BtnSortContracts_Click(object sender, RoutedEventArgs e)
        {
            var contracts = (List<Contract>)GridContracts.ItemsSource;
            GridContracts.ItemsSource = contracts.OrderBy(x => x.ContractDate).ToList();
        }

        private void CmbQueries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CmbQueries.SelectedIndex == 2)
                PanelQueryParams.Visibility = Visibility.Visible;
            else
                PanelQueryParams.Visibility = Visibility.Collapsed;

            GridQueryResults.ItemsSource = null;
        }

        private void BtnExecuteQuery_Click(object sender, RoutedEventArgs e)
        {
            int index = CmbQueries.SelectedIndex;
            if (index == -1) return;

            switch (index)
            {
                case 0:
                    GridQueryResults.ItemsSource = _empRepo.GetWithHigherEducation();
                    break;
                case 1:
                    GridQueryResults.ItemsSource = _srvRepo.GetPopularServices();
                    break;
                case 2:
                    if (CmbQueryEmployee.SelectedValue == null)
                    {
                        MessageBox.Show("Select Employee");
                        return;
                    }
                    GridQueryResults.ItemsSource = _cntRepo.GetByEmployee((int)CmbQueryEmployee.SelectedValue);
                    break;
                case 3:
                    GridQueryResults.ItemsSource = _cntRepo.GetEmployeeStats();
                    break;
                case 4:
                    GridQueryResults.ItemsSource = _cntRepo.GetServiceDateStats();
                    break;
            }
        }

        private void BtnExportQuery_Click(object sender, RoutedEventArgs e)
        {
            if (GridQueryResults.ItemsSource == null) return;
            int index = CmbQueries.SelectedIndex;

            if (index == 2)
            {
                var contracts = (List<Contract>)GridQueryResults.ItemsSource;
                var empName = ((Employee)CmbQueryEmployee.SelectedItem).FullName;
                _reportGen.GenerateEmployeeContractsReport(empName, contracts);
            }
            else if (index == 3)
            {
                var stats = (IEnumerable)GridQueryResults.ItemsSource;
                _reportGen.GenerateStatsReport(stats);
            }
            else
            {
                MessageBox.Show("Generic Export not implemented");
            }
        }
    }
}
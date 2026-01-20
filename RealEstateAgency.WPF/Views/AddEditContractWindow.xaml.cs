using System;
using System.Windows;
using RealEstateAgency.DataAccess.Models;
using RealEstateAgency.DataAccess.Repositories;

namespace RealEstateAgency.WPF.Views
{
    public partial class AddEditContractWindow : Window
    {
        public Contract ContractObj { get; private set; }
        private EmployeeRepository _empRepo = new EmployeeRepository();
        private ServiceRepository _srvRepo = new ServiceRepository();

        public AddEditContractWindow(Contract contract = null)
        {
            InitializeComponent();
            LoadCombos();

            if (contract != null)
            {
                ContractObj = contract;
                TxtNumber.Text = contract.ContractNumber;
                DpDate.SelectedDate = contract.ContractDate;
                TxtClient.Text = contract.ClientName;
                TxtClientPhone.Text = contract.ClientPhone;
                CmbEmployees.SelectedValue = contract.EmployeeId;
                CmbServices.SelectedValue = contract.ServiceId;
            }
            else
            {
                ContractObj = new Contract();
                DpDate.SelectedDate = DateTime.Now;
            }
        }

        private void LoadCombos()
        {
            CmbEmployees.ItemsSource = _empRepo.GetAll();
            CmbEmployees.DisplayMemberPath = "FullName";
            CmbEmployees.SelectedValuePath = "Id";

            CmbServices.ItemsSource = _srvRepo.GetAll();
            CmbServices.DisplayMemberPath = "Name";
            CmbServices.SelectedValuePath = "Id";
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (CmbEmployees.SelectedValue == null || CmbServices.SelectedValue == null)
            {
                MessageBox.Show("Выберите сотрудника и услугу");
                return;
            }

            ContractObj.ContractNumber = TxtNumber.Text;
            ContractObj.ContractDate = DpDate.SelectedDate ?? DateTime.Now;
            ContractObj.ClientName = TxtClient.Text;
            ContractObj.ClientPhone = TxtClientPhone.Text;
            ContractObj.EmployeeId = (int)CmbEmployees.SelectedValue;
            ContractObj.ServiceId = (int)CmbServices.SelectedValue;

            DialogResult = true;
        }
    }
}
using System;
using System.Linq;
using System.Windows;
using RealEstateAgency.EF.DataAccess.Data;
using RealEstateAgency.EF.DataAccess.Models;

namespace RealEstateAgency.EF.WPF.Views
{
    public partial class AddEditContractWindow : Window
    {
        public Contract ContractObj { get; private set; }

        public AddEditContractWindow(RealEstateDbContext context, Contract contract = null)
        {
            InitializeComponent();

            CmbEmployees.ItemsSource = context.Employees.ToList();
            CmbEmployees.DisplayMemberPath = "FullName";
            CmbEmployees.SelectedValuePath = "Id";

            CmbServices.ItemsSource = context.Services.ToList();
            CmbServices.DisplayMemberPath = "Name";
            CmbServices.SelectedValuePath = "Id";

            if (contract != null)
            {
                ContractObj = contract;
                TxtNumber.Text = contract.ContractNumber;
                DpDate.SelectedDate = contract.ContractDate;
                TxtClient.Text = contract.ClientName;
                TxtPhone.Text = contract.ClientPhone;
                CmbEmployees.SelectedValue = contract.EmployeeId;
                CmbServices.SelectedValue = contract.ServiceId;
            }
            else
            {
                ContractObj = new Contract();
                DpDate.SelectedDate = DateTime.Now;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (CmbEmployees.SelectedValue == null || CmbServices.SelectedValue == null)
            {
                MessageBox.Show("Select Employee and Service");
                return;
            }

            ContractObj.ContractNumber = TxtNumber.Text;
            ContractObj.ContractDate = DpDate.SelectedDate ?? DateTime.Now;
            ContractObj.ClientName = TxtClient.Text;
            ContractObj.ClientPhone = TxtPhone.Text;
            ContractObj.EmployeeId = (int)CmbEmployees.SelectedValue;
            ContractObj.ServiceId = (int)CmbServices.SelectedValue;

            DialogResult = true;
        }
    }
}
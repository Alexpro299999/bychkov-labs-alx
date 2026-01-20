using System.Windows;
using RealEstateAgency.DataAccess.Models;

namespace RealEstateAgency.WPF.Views
{
    public partial class AddEditEmployeeWindow : Window
    {
        public Employee Employee { get; private set; }

        public AddEditEmployeeWindow(Employee emp = null)
        {
            InitializeComponent();
            if (emp != null)
            {
                Employee = emp;
                TxtName.Text = emp.FullName;
                TxtPosition.Text = emp.Position;
                TxtAddress.Text = emp.Address;
                TxtPhone.Text = emp.Phone;
                TxtEdu.Text = emp.Education;
                TxtSpec.Text = emp.Specialty;
            }
            else
            {
                Employee = new Employee();
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            Employee.FullName = TxtName.Text;
            Employee.Position = TxtPosition.Text;
            Employee.Address = TxtAddress.Text;
            Employee.Phone = TxtPhone.Text;
            Employee.Education = TxtEdu.Text;
            Employee.Specialty = TxtSpec.Text;
            DialogResult = true;
        }
    }
}
using System.Windows;
using RealEstateAgency.DataAccess.Models;

namespace RealEstateAgency.WPF.Views
{
    public partial class AddEditServiceWindow : Window
    {
        public Service ServiceObj { get; private set; }

        public AddEditServiceWindow(Service srv = null)
        {
            InitializeComponent();
            if (srv != null)
            {
                ServiceObj = srv;
                TxtName.Text = srv.Name;
                TxtCost.Text = srv.Cost.ToString();
            }
            else
            {
                ServiceObj = new Service();
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            ServiceObj.Name = TxtName.Text;
            if (decimal.TryParse(TxtCost.Text, out decimal cost))
                ServiceObj.Cost = cost;
            else
                ServiceObj.Cost = 0;

            DialogResult = true;
        }
    }
}
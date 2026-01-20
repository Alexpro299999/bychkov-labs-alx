using System.Windows;

namespace RealEstateAgency.WPF.Views
{
    public partial class InputDialog : Window
    {
        public string InputText { get; private set; }

        public InputDialog(string prompt)
        {
            InitializeComponent();
            LblPrompt.Text = prompt;
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            InputText = TxtInput.Text;
            DialogResult = true;
        }
    }
}
using System.Windows;
using RealEstateAgency.DataAccess;

namespace RealEstateAgency.WPF
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            DatabaseInitializer.Initialize();
        }
    }
}
using System.Windows;
using RealEstateAgency.EF.DataAccess.Data;

namespace RealEstateAgency.EF.WPF
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            using (var context = new RealEstateDbContext())
            {
                DbInitializer.Initialize(context);
            }
        }
    }
}
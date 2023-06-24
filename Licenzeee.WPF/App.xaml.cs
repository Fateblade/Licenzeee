using Fateblade.Licenzee.Db;
using Fateblade.Licenzeee.WPF.Db;
using Prism.Ioc;
using System.Windows;

namespace Fateblade.Licenzeee.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : IContainerApp
    {
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register(typeof(IDbMigrator), typeof(DbMigrator));
            containerRegistry.RegisterSingleton(typeof(IDbProvider), typeof(DbProvider));
        }
        
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }
    }
}

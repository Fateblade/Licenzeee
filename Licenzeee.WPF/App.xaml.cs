using Fateblade.Licenzee.Db;
using Fateblade.Licenzeee.WPF.Db;
using Fateblade.Licenzeee.WPF.Events;
using Prism.Events;
using Prism.Ioc;
using System.Windows;
using System.Windows.Navigation;

namespace Fateblade.Licenzeee.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : IDbProvidingApp
    {
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register(typeof(IDbMigrator), typeof(DbMigrator));
            containerRegistry.RegisterInstance(typeof(IDbProvidingApp), this);
        }

        protected override void OnLoadCompleted(NavigationEventArgs e)
        {
            base.OnLoadCompleted(e);
            
            Container.Resolve<IEventAggregator>().GetEvent<PubSubEvent<ChangeDbRequest>>().Subscribe(handleChangeDbRequest);
        }

        private void handleChangeDbRequest(ChangeDbRequest obj)
        {
            if (obj.MigrateExistingData)
            {
                Container.Resolve<IDbMigrator>().Migrate(Db, obj.NewDbToUse);
            }

            Db = obj.NewDbToUse;

            Container.Resolve<IEventAggregator>().GetEvent<PubSubEvent<DbChanged>>().Publish(new DbChanged(Db));
        }

        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        public IDb Db { get; private set; } = new InMemoryDb();
    }

}

using Fateblade.Licenzee.Db;
using Fateblade.Licenzee.Db.SqLite;
using Fateblade.Licenzeee.WPF.Events;
using Prism.Events;
using System;
using Fateblade.Licenzeee.MySql;

namespace Fateblade.Licenzeee.WPF.Db
{
    internal class DbProvider : IDbProvider
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IDbMigrator _dbMigrator;


        public IDb Db { get; private set; } = new InMemoryDb();


        public DbProvider(IEventAggregator eventAggregator, IDbMigrator dbMigrator)
        {
            _eventAggregator = eventAggregator;
            _dbMigrator = dbMigrator;

            eventAggregator.GetEvent<PubSubEvent<ChangeDbRequest>>().Subscribe(handleChangeDbRequest);

            initializeDb();
        }

        private void initializeDb()
        {
            if (Properties.DatabaseSettings.Default.UseSqLiteDb)
            {
                Db = new SqLiteDb(Properties.DatabaseSettings.Default.SqLiteDbPath);
            }
        }


        private void handleChangeDbRequest(ChangeDbRequest obj)
        {
            IDb? newDbToUse = null;
            switch (obj.NewDbToUse)
            {
                case KnownDbTypes.InMemoryOnly:
                    newDbToUse = new InMemoryDb();
                    break;
                case KnownDbTypes.SqLite:
                    newDbToUse = new SqLiteDb(Properties.DatabaseSettings.Default.SqLiteDbPath);
                    break;
                case KnownDbTypes.MySql:
                    newDbToUse = new MySqlDb(
                        Properties.DatabaseSettings.Default.MySqlServerName,
                        Properties.DatabaseSettings.Default.MySqlDatabaseName,
                        Properties.DatabaseSettings.Default.MySqlUserId,
                        "");//TODO: Request Password from user
                    break;
                default: throw new ArgumentException($"Unknown db type '{obj.NewDbToUse}'");
            }

            if (obj.MigrateExistingData)
            {
                _dbMigrator.Migrate(Db, newDbToUse);
            }

            Db = newDbToUse;

            _eventAggregator.GetEvent<PubSubEvent<DbChanged>>().Publish(new DbChanged(Db));
        }

    }
}

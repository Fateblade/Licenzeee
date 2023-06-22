using Fateblade.Licenzee.Db;

namespace Fateblade.Licenzeee.WPF.Db
{
    interface IDbMigrator
    {
        void Migrate(IDb oldDb, IDb newDb);
    }

    internal class DbMigrator : IDbMigrator
    {
        public void Migrate(IDb oldDb, IDb newDb)
        {
            throw new System.NotImplementedException();
        }
    }
}

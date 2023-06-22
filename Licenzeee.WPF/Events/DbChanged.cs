using Fateblade.Licenzee.Db;

namespace Fateblade.Licenzeee.WPF.Events
{
    internal class DbChanged
    {
        private IDb NewDbInstance { get; }

        public DbChanged(IDb newDbInstance)
        {
            NewDbInstance = newDbInstance;
        }
    }
}

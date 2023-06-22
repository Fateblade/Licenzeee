using Fateblade.Licenzee.Db;

namespace Fateblade.Licenzeee.WPF.Events
{
    internal class ChangeDbRequest
    {
        public IDb NewDbToUse { get; }
        public bool MigrateExistingData { get; }

        public ChangeDbRequest(IDb newDbToUse, bool migrateExistingData)
        {
            NewDbToUse = newDbToUse;
            MigrateExistingData = migrateExistingData;
        }
    }
}

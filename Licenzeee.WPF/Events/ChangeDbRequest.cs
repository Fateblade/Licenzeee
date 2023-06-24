using Fateblade.Licenzee.Db;

namespace Fateblade.Licenzeee.WPF.Events
{
    enum KnownDbTypes
    {
        InMemoryOnly,
        SqLite
    }
    internal class ChangeDbRequest
    {
        public KnownDbTypes NewDbToUse { get; }
        public bool MigrateExistingData { get; }

        public ChangeDbRequest(KnownDbTypes newDbToUse, bool migrateExistingData)
        {
            NewDbToUse = newDbToUse;
            MigrateExistingData = migrateExistingData;
        }
    }
}

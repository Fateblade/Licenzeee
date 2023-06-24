using Fateblade.Licenzee.Db;
using Fateblade.Licenzeee.WPF.Events;
using Prism.Events;

namespace Fateblade.Licenzeee.WPF.Db
{
    internal class DbImportHandler
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IDb _db;


        public DbImportHandler(IEventAggregator eventAggregator, IDbProvider dbProvider)
        {
            _eventAggregator = eventAggregator;
            _db = dbProvider.Db;


            _eventAggregator.GetEvent<PubSubEvent<ImportSampleDataRequest>>().Subscribe(handleImportSampleDataRequest);
        }


        private void handleImportSampleDataRequest(ImportSampleDataRequest obj)
        {
            new SampleDataCreator(_db).ImportSampleData();

            _eventAggregator.GetEvent<PubSubEvent<NewDataImported>>().Publish(new NewDataImported());
            _eventAggregator.GetEvent<PubSubEvent<UserInfoRequest>>().Publish(new UserInfoRequest("Import completed", "Sample data was imported"));
        }
    }
}

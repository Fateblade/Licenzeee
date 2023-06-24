using System.Configuration;
using Fateblade.Licenzeee.WPF.Properties;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.IO;
using Fateblade.Licenzeee.WPF.Events;
using Fateblade.Licenzeee.WPF.Inputs;

namespace Fateblade.Licenzeee.WPF.Views
{
    internal class OptionsViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly string _defaultPath;

        private string _dbPath = string.Empty;
        public string SqLiteDbPath
        {
            get => _dbPath;
            set => SetProperty(ref _dbPath, value);
        }

        private bool _useInMemoryDb;
        public bool UseInMemoryDb
        {
            get => _useInMemoryDb;
            set => SetProperty(ref _useInMemoryDb, value);
        }

        private bool _useSqLiteDb;
        public bool UseSqLiteDb
        {
            get => _useSqLiteDb;
            set => SetProperty(ref _useSqLiteDb, value);
        }

        private bool _currentlySaving;
        public bool CurrentlySaving
        {
            get => _currentlySaving;
            set => SetProperty(ref _currentlySaving, value);
        }

        private bool _currentlyImporting;
        public bool CurrentlyImporting
        {
            get => _currentlyImporting;
            set => SetProperty(ref _currentlyImporting, value);
        }

        public DelegateCommand SelectDbPath { get; }
        public DelegateCommand SaveSettings { get; }
        public DelegateCommand ImportSampleData { get; }


        public OptionsViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _defaultPath = Path.Combine(Directory.GetCurrentDirectory(), "data.db");

            SelectDbPath = new DelegateCommand(openFileDialog);
            SaveSettings = new DelegateCommand(applySettings, canSaveSettings)
                .ObservesProperty(() => UseInMemoryDb)
                .ObservesProperty(() => UseSqLiteDb)
                .ObservesProperty(() => SqLiteDbPath)
                .ObservesProperty(() => CurrentlySaving)
                .ObservesProperty(() => CurrentlyImporting);
            ImportSampleData = new DelegateCommand(importSampleData, ()=>!CurrentlySaving)
                .ObservesProperty(()=> CurrentlySaving);

            UseInMemoryDb = DatabaseSettings.Default.UseInMemoryDb;
            UseSqLiteDb = DatabaseSettings.Default.UseSqLiteDb;
            SqLiteDbPath = string.IsNullOrWhiteSpace(DatabaseSettings.Default.SqLiteDbPath)
                ? _defaultPath
                : DatabaseSettings.Default.SqLiteDbPath;
        }


        private void openFileDialog()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;
            dialog.CheckFileExists = false;
            dialog.Filter = "Database files (*.db)|*.db|All files (*.*)|*.*";
            dialog.InitialDirectory = string.IsNullOrWhiteSpace(SqLiteDbPath)
                ? (Directory.GetParent(SqLiteDbPath)?.ToString() ?? Directory.GetCurrentDirectory())
                : Directory.GetCurrentDirectory();
            var result = dialog.ShowDialog();

            if (result==true)
            {
                SqLiteDbPath = dialog.FileName;
            }
        }

        private bool canSaveSettings()
        {
            return !CurrentlySaving
                   && !CurrentlyImporting
                   && dbSettingsChanged();
        }
        
        private bool dbSettingsChanged()
        {
            return DatabaseSettings.Default.UseInMemoryDb != UseInMemoryDb
                   || DatabaseSettings.Default.UseSqLiteDb != UseSqLiteDb
                   || (UseSqLiteDb && !DatabaseSettings.Default.SqLiteDbPath.ToLower().Equals(SqLiteDbPath?.ToLower()));
        }

        private void applySettings()
        {
            CurrentlySaving = true;

            if (dbSettingsChanged())
            {
                saveSettings();
                _eventAggregator.GetEvent<PubSubEvent<UserYesNoRequest>>().Publish(
                    new UserYesNoRequest(
                        "Migrate Data?",
                        "Should the existing data be migrated into the new database?",
                        applySettings_MigrateDataInputRequest));
            }
            else
            {
                saveSettings();
                CurrentlySaving = false;
            }
        }

        private void saveSettings()
        {
            DatabaseSettings.Default.UseInMemoryDb = UseInMemoryDb;
            DatabaseSettings.Default.UseSqLiteDb = UseSqLiteDb;
            DatabaseSettings.Default.SqLiteDbPath = SqLiteDbPath;
            DatabaseSettings.Default.Save();
        }

        private void applySettings_MigrateDataInputRequest(bool migrateData)
        {
            _eventAggregator.GetEvent<PubSubEvent<NewDataImported>>().Subscribe(applySettings_NewDataImported);
            _eventAggregator.GetEvent<PubSubEvent<ChangeDbRequest>>().Publish(new ChangeDbRequest(UseInMemoryDb ? KnownDbTypes.InMemoryOnly : KnownDbTypes.SqLite, migrateData));
        }

        private void applySettings_NewDataImported(NewDataImported obj)
        {
            _eventAggregator.GetEvent<PubSubEvent<NewDataImported>>().Unsubscribe(applySettings_NewDataImported);
            CurrentlySaving = false;
        }

        private void importSampleData()
        {
            if (dbSettingsChanged())
            {
                _eventAggregator.GetEvent<PubSubEvent<UserInfoRequest>>().Publish(
                    new UserInfoRequest(
                        "Info", 
                        "Please save or undo the setting changes to the database before importing sample data"));
            }
            else
            {
                CurrentlyImporting = true;
                _eventAggregator.GetEvent<PubSubEvent<NewDataImported>>().Subscribe(importSampleData_NewDataImported);
                _eventAggregator.GetEvent<PubSubEvent<ImportSampleDataRequest>>().Publish(new ImportSampleDataRequest());
            }
        }

        private void importSampleData_NewDataImported(NewDataImported obj)
        {
            _eventAggregator.GetEvent<PubSubEvent<NewDataImported>>().Unsubscribe(importSampleData_NewDataImported);
            CurrentlyImporting = false;
        }
    }
}

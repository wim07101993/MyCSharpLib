using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MyCSharpLib.Services
{
    /// <summary>
    /// Provides all the strings of the application in a specified language.
    /// </summary>
    public class StringsProvider<T> : AStringsProvider<T> where T : IStrings, new()
    {
        #region FIELDS

        /// <summary>Service to read and write the strings to files.</summary>
        private readonly IFileService _fileService;

        /// <summary>Field for the <see cref="Strings"/> property.</summary>
        private IStringsSettings _settings;

        /// <summary>Field for the the <see cref="Languages"/> property.</summary>
        private string[] _languages;

        #endregion FIELDS


        #region CONSTRUCTOR

        public StringsProvider(ISettingsProvider<IStringsSettings> settingsProvider, IFileService fileService)
        {
            _settings = settingsProvider.Settings;
            _fileService = fileService;

            if (!Directory.Exists(_settings.LanguagesDirectory))
                Directory.CreateDirectory(_settings.LanguagesDirectory);

            FetchLanguagePossibilities();
            settingsProvider.Settings.PropertyChanged += OnSettingsPropertyChanged;
        }

        #endregion CONSTRUCTOR


        #region PROPERTIES
        
        public override string[] Languages
        {
            get => _languages;
            protected set => SetProperty(ref _languages, value);
        }

        #endregion PROPERTIES


        #region METHODS

        protected override async Task<T> InternalFetchStringsAsync(string language) 
        {
            return await _fileService.ReadAsync<T>($@"{_settings.LanguagesDirectory}\{language}");
        }

        protected override async Task InternalSaveStringsAsync(T strings, string language)
        {
            await _fileService.WriteAsync(strings, $@"{_settings.LanguagesDirectory}\{language}");
        }
        
        public void FetchLanguagePossibilities()
        {
            Languages = Directory
                .GetFiles(_settings.LanguagesDirectory)
                .Select(x => x.Split('/').Last())
                .ToArray();
        }

        protected override void OnSettingsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var settings = (T)sender;
            switch (e.PropertyName)
            {
                case nameof(Settings.Settings.LanguagesDirectory):
                    FetchLanguagePossibilities();
                    break;
            }
        }

        #endregion METHODS
    }
}

using MyCSharpLib.Services.Serialization;
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
        
        /// <summary>Field for the the <see cref="Languages"/> property.</summary>
        private string[] _languages;

        #endregion FIELDS


        #region CONSTRUCTOR

        public StringsProvider(IStringsFileSettings settings, ISerializerDeserializer serializerDeserializer)
            : base(settings)
        {
            SerializerDeserializer = serializerDeserializer;

            if (!Directory.Exists(Settings.LanguagesDirectory))
                Directory.CreateDirectory(Settings.LanguagesDirectory);

            FetchLanguagePossibilities();
            Settings.PropertyChanged += OnSettingsPropertyChanged;
        }

        #endregion CONSTRUCTOR


        #region PROPERTIES

        public ISerializerDeserializer SerializerDeserializer { get; }

        /// <summary>Field for the <see cref="Strings"/> property.</summary>
        private IStringsFileSettings Settings => settings as IStringsFileSettings;

        public override string[] Languages
        {
            get => _languages;
            protected set => SetProperty(ref _languages, value);
        }

        #endregion PROPERTIES


        #region METHODS

        protected override async Task<T> InternalFetchStringsAsync(string language) 
        {
            var path = $@"{Settings.LanguagesDirectory}\{language}.{((IDeserializer)SerializerDeserializer).FileExtension}";

            using (var fileStream = File.OpenText(path))
                return await SerializerDeserializer.DeserializeAsync<T>(fileStream);
        }

        protected override async Task InternalSaveStringsAsync(T strings, string language)
        {
            var path = $@"{Settings.LanguagesDirectory}\{language}.{((ISerializer)SerializerDeserializer).FileExtension}";

            using (var writer = new StreamWriter(path))
                await SerializerDeserializer.SerializeAsync(strings, writer);
        }
        
        public void FetchLanguagePossibilities()
        {
            Languages = Directory
                .GetFiles(Settings.LanguagesDirectory)
                .Select(Path.GetFileNameWithoutExtension)
                .ToArray();
        }

        protected override void OnSettingsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Settings.LanguagesDirectory):
                    FetchLanguagePossibilities();
                    break;
            }
        }
        
        #endregion METHODS
    }
}

namespace MyCSharpLib.Services
{
    public class StringsFileSettings : StringsSettings, IStringsFileSettings
    {
        private string _languageDirectory;
        
        public string LanguagesDirectory
        {
            get => _languageDirectory;
            set => SetProperty(ref _languageDirectory, value);
        }
    }
}

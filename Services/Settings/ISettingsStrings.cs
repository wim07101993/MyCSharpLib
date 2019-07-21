namespace MyCSharpLib.Services
{
    public interface ISettingsStrings : IStrings
    {
        string Settings { get; set; }
        string SaveSettings { get; set; }
        string SaveSettingsTooltip { get; set; }

        string ApplicationDataDirectory { get; set; }
        string SettingsPath { get; set; }
        string LogDirectory { get; set; }
        string LanguagesDirectory { get; set; }

        string Language { get; set; }
        string DarkMode { get; set; }
        string WrongSettingsTypeExceptionMessage { get; set; }

    }
}

namespace MyCSharpLib.Services
{
    /// <summary>
    /// Interface to determine what settings are needed for the <see cref="StringsProvider{T}"/>.
    /// </summary>
    public class StringsSettings : Settings
    {
        /// <summary>The current language of the application.</summary>
        public string Language { get; set; } = "eng";

        /// <summary>Directory where the strings are stored.</summary>
        public string LanguagesDirectory { get; set; } = $@"{FileServiceSettings.ApplicationDataDirectory}\Languages";
    }
}

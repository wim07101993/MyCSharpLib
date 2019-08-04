namespace MyCSharpLib.Services
{
    public interface IStringsFileSettings : IStringsSettings
    {
        /// <summary>Directory where the strings are stored.</summary>
        string LanguagesDirectory { get; set; }
    }
}

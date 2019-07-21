namespace MyCSharpLib.Services
{
    /// <summary>
    /// Interface for the settings of the application.
    /// </summary>
    public interface ISettings
    {
        /// <summary>
        /// Vendor of the application. Should be set before using the service. (Ex.: in App.xaml.cs or program.cs)
        /// </summary>
        string Vendor { get; set; }

        /// <summary>
        /// Name of the application.
        /// </summary>
        string ApplicationName { get; }
    }
}

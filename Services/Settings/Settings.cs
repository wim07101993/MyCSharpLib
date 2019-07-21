using Newtonsoft.Json;
using Prism.Mvvm;
using System.ComponentModel;
using System.Reflection;

namespace MyCSharpLib.Services
{
    /// <inheritdoc />
    /// <summary>
    /// Settings for the application.
    /// Extends from the <see cref="BindableBase"/>.
    /// </summary>
    public class Settings : BindableBase, ISettings
    {
        /// <summary>
        /// Vendor of the application. Should be set before using the service. (Ex.: in App.xaml.cs or program.cs)
        /// </summary>
        [JsonIgnore]
        [Browsable(false)]
        public static string Vendor { get; set; }

        /// <summary>
        /// Vendor of the application. Should be set before using the service. (Ex.: in App.xaml.cs or program.cs)
        /// </summary>
        [JsonIgnore]
        [Browsable(false)]
        string ISettings.Vendor
        {
            get => Vendor;
            set => Vendor = value;
        }

        /// <summary>
        /// Name of the application.
        /// </summary>
        [JsonIgnore]
        [Browsable(false)]
        public static string ApplicationName => Assembly.GetEntryAssembly().GetName().Name;

        /// <summary>
        /// Name of the application.
        /// </summary>
        [JsonIgnore]
        [Browsable(false)]
        string ISettings.ApplicationName => ApplicationName;
    }
}

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
    public class Settings : BindableBase
    {
        #region FIELDS

        private FileServiceSettings _fileSettings;

        #endregion FIELDS


        #region PROPERTIES

        [JsonIgnore]
        [Browsable(false)]
        public static string Vendor { get; set; }

        [JsonIgnore]
        [Browsable(false)]
        public static string ApplicationName => Assembly.GetEntryAssembly().GetName().Name;

        public FileServiceSettings FileSettings
        {
            get => _fileSettings;
            set => SetProperty(ref _fileSettings, value);
        }

        #endregion PROPERTIES
    }
}

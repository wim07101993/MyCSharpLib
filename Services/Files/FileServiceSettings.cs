using Newtonsoft.Json;
using Prism.Mvvm;
using System;
using System.Collections.Generic;

namespace MyCSharpLib.Services
{
    /// <summary>
    /// Class that contains the settings needed to get the <see cref="FileService"/> working.
    /// This class should be used in the <see cref="Settings"/> class for the <see cref="ISettingsProvider"/>.
    /// Extends <see cref="BindableBase"/>.
    /// </summary>
    public class FileServiceSettings : Settings, IFileServiceSettings
    {
        #region FIELDS

        /// <summary>Path to directory in which the data of the application is stored.</summary>
        private string _dataDirectory = $@"{ApplicationDataDirectory}\Data";

        /// <summary>Dictionary that contains the different file paths to different types of data.</summary>
        private Dictionary<Type, string> _filePaths;

        #endregion FIELDS


        #region PROPERTIES

        /// <summary>Path that points to the {AppData}/{vendor}/{application name}.</summary>
        [JsonIgnore]
        public static string ApplicationDataDirectory => $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\{Settings.Vendor}\{Settings.ApplicationName}";

        /// <summary>Path to the settings of the application. ({ApplicationDataDirectory}/settings.json)</summary>
        [JsonIgnore]
        public static string SettingsPath => $@"{ApplicationDataDirectory}\settings.json";

        /// <summary>Path to directory in which the data of the application is stored.</summary>
        public string DataDirectory
        {
            get => _dataDirectory;
            set => SetProperty(ref _dataDirectory, value);
        }

        /// <summary>Dictionary that contains the different file paths to different types of data.</summary>
        public Dictionary<Type, string> FilePaths
        {
            get => _filePaths;
            set => SetProperty(ref _filePaths, value);
        }

        #endregion PROPERTIES
    }
}

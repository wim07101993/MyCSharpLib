using Newtonsoft.Json;
using Prism.Mvvm;
using System;
using System.Collections.Generic;

namespace MyCSharpLib.Services
{
    /// <summary>
    /// Class that contains the settings needed to get the <see cref="FileService"/> working.
    /// Extends <see cref="BindableBase"/>.
    /// </summary>
    public class FileServiceSettings : BindableBase, IFileServiceSettings
    {
        #region FIELDS

        private readonly string _vendor;
        private readonly string _applicationName;

        /// <summary>Path to directory in which the data of the application is stored.</summary>
        private string _dataDirectory;

        /// <summary>Dictionary that contains the different file paths to different types of data.</summary>
        private Dictionary<Type, string> _filePaths;

        #endregion FIELDS


        #region CONSTRUCTOR

        public FileServiceSettings(string vendor, string applicationName)
        {
            _applicationName = applicationName;
            _vendor = vendor;

            if (string.IsNullOrEmpty(_dataDirectory))
                _dataDirectory = $@"{ApplicationDataDirectory}\Data";
        }

        #endregion CONSTRUCTOR


        #region PROPERTIES

        /// <summary>Path that points to the {AppData}/{vendor}/{application name}.</summary>
        [JsonIgnore]
        public string ApplicationDataDirectory => $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\{_vendor}\{_applicationName}";

        /// <summary>Path to the settings of the application. ({ApplicationDataDirectory}/settings.json)</summary>
        [JsonIgnore]
        public string SettingsPath => $@"{ApplicationDataDirectory}\settings.json";

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

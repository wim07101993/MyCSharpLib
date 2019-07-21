using Newtonsoft.Json;
using Prism.Mvvm;
using System;
using System.Collections.Generic;

namespace MyCSharpLib.Services
{
    public class FileServiceSettings : BindableBase
    {

        private string _dataDirectory = $@"{ApplicationDataDirectory}\Data";
        private Dictionary<Type, string> _filePaths;


        [JsonIgnore]
        public static string ApplicationDataDirectory => $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\{Settings.Vendor}\{Settings.ApplicationName}";

        [JsonIgnore]
        public static string SettingsPath => $@"{ApplicationDataDirectory}\settings.json";


        public string DataDirectory
        {
            get => _dataDirectory;
            set => SetProperty(ref _dataDirectory, value);
        }

        public Dictionary<Type, string> FilePaths
        {
            get => _filePaths;
            private set => SetProperty(ref _filePaths, value);
        }
    }
}

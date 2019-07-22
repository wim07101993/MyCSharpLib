using Newtonsoft.Json;
using Prism.Mvvm;
using System;
using System.Collections.Generic;

namespace MyCSharpLib.Services
{
    /// <summary>
    /// Interface that describes the settings needed to get the <see cref="FileService"/> working.
    /// This class should be used in the <see cref="Settings"/> class for the <see cref="ISettingsProvider"/>.
    /// Extends <see cref="BindableBase"/>.
    /// </summary>
    public interface IFileServiceSettings : ISettings
    {
        string DataDirectory { get; set; }

        /// <summary>Dictionary that contains the different file paths to different types of data.</summary>
        Dictionary<Type, string> FilePaths { get; set; }
    }
}

using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace WSharp.Files
{
    /// <summary>
    ///     Interface that describes the settings needed to get the <see cref="FileService"/>
    ///     working. Extends <see cref="BindableBase"/>.
    /// </summary>
    public interface IFileServiceSettings : INotifyPropertyChanged
    {
        string DataDirectory { get; set; }

        /// <summary>
        ///     Dictionary that contains the different file paths to different types of data.
        /// </summary>
        Dictionary<Type, string> FilePaths { get; set; }
    }
}
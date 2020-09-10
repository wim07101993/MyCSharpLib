using System;
using System.Collections.Generic;

namespace WSharp.Dialogs.Options
{
    public struct SaveFileDialogOptions : ISaveFileDialogOptions
    {
        public SaveFileDialogOptions(
            IList<FileFilter> filter, Type valueType, string originalFilePath,
            bool addAllFilesFilterOption = default, string initialDirectory = default)
        {
            Filter = filter;
            AddAllFilesFilterOption = addAllFilesFilterOption;
            OriginalFilePath = originalFilePath;
            InitialDirectory = initialDirectory;
            ValueType = valueType;
        }

        public IList<FileFilter> Filter { get; }

        public bool AddAllFilesFilterOption { get; }

        public string OriginalFilePath { get; }

        public string InitialDirectory { get; }

        public Type ValueType { get; }
    }
}

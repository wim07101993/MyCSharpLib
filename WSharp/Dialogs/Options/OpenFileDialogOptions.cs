using System;
using System.Collections.Generic;

namespace WSharp.Dialogs.Options
{
    public struct OpenFileDialogOptions : IOpenFileDialogOptions
    {
        public OpenFileDialogOptions(
            IList<FileFilter> filter,
            bool addAllFilesFilterOption,
            string initialDirectory,
            Type valueType,
            bool multiSelect)
        {
            Filter = filter;
            AddAllFilesFilterOption = addAllFilesFilterOption;
            InitialDirectory = initialDirectory;
            ValueType = valueType;
            MultiSelect = multiSelect;
        }

        public IList<FileFilter> Filter { get; }

        public bool AddAllFilesFilterOption { get; }

        public string InitialDirectory { get; }

        public Type ValueType { get; }

        public bool MultiSelect { get; }
    }
}

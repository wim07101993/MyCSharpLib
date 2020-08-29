using System;
using System.Collections.Generic;

namespace WSharp.Dialogs.Options
{
    public interface IOpenFileDialogOptions
    {
        IList<FileFilter> Filter { get; }
        bool AddAllFilesFilterOption { get; }
        string InitialDirectory { get; }
        Type ValueType { get; }
        bool MultiSelect { get; }
    }
}

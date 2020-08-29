using System;
using System.Collections.Generic;

namespace WSharp.Dialogs.Options
{
    public interface ISaveFileDialogOptions
    {
        IList<FileFilter> Filter { get; }
        bool AddAllFilesFilterOption { get; }
        string OriginalFilePath { get; }
        string InitialDirectory { get; }
        Type ValueType { get; }
    }
}

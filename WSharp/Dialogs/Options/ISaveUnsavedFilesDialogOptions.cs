using System.Collections.Generic;

using WSharp.Files;

namespace WSharp.Dialogs.Options
{
    public interface ISaveUnsavedFilesDialogOptions
    {
        IReadOnlyCollection<IFile> FilesToSave { get; }
    }
}

using System.Collections.Generic;

using WSharp.Files;

namespace WSharp.Dialogs.Options
{
    public interface ISaveUnsavedFilesDialogResult
    {
        EDialogResult Result { get; }

        IReadOnlyCollection<(IFile file, bool shouldSave)> FilesToSave { get; }
    }
}

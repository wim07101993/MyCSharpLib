using System.Collections.Generic;

using WSharp.Dialogs.Options;
using WSharp.Files;

namespace WSharp.Dialogs.Results
{
    public struct SaveUnsavedFilesDialogResult : ISaveUnsavedFilesDialogResult
    {
        public SaveUnsavedFilesDialogResult(EDialogResult result, IReadOnlyCollection<(IFile file, bool shouldSave)> filesToSave)
        {
            Result = result;
            FilesToSave = filesToSave;
        }

        public EDialogResult Result { get; }

        public IReadOnlyCollection<(IFile file, bool shouldSave)> FilesToSave { get; }
    }
}

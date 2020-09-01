using System.Collections.Generic;

using WSharp.Files;

namespace WSharp.Dialogs.Options
{
    public struct SaveUnsavedFilesDialogOptions : ISaveUnsavedFilesDialogOptions
    {
        public SaveUnsavedFilesDialogOptions(IReadOnlyCollection<IFile> filesToSave)
        {
            FilesToSave = filesToSave;
        }

        public IReadOnlyCollection<IFile> FilesToSave { get; }
    }
}

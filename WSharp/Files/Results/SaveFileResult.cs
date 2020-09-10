using WSharp.Dialogs;

namespace WSharp.Files.Results
{
    public struct SaveFileResult : ISaveFileResult
    {
        public SaveFileResult(EDialogResult dialogResult, IFile file)
        {
            File = file;
            DialogResult = dialogResult;
        }

        public IFile File { get; }

        public EDialogResult DialogResult { get; }
    }
}

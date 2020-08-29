using WSharp.Dialogs;

namespace WSharp.Files.Results
{
    public struct MultipleOpenFileResult : IMultipleOpenFileResult
    {
        public MultipleOpenFileResult(EDialogResult dialogResult, IFile[] files)
        {
            Files = files;
            DialogResult = dialogResult;
        }

        public IFile[] Files { get; }

        public EDialogResult DialogResult { get; }
    }
}

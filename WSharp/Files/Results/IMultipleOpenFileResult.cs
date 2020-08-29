using WSharp.Dialogs;

namespace WSharp.Files.Results
{
    public interface IMultipleOpenFileResult
    {
        IFile[] Files { get; }
        EDialogResult DialogResult { get; }
    }
}

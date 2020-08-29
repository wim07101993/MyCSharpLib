using WSharp.Dialogs;

namespace WSharp.Files.Results
{
    public interface ISaveFileResult
    {
        IFile File { get; }
        EDialogResult DialogResult { get; }
    }
}

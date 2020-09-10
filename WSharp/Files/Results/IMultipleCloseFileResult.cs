using WSharp.Dialogs;

namespace WSharp.Files.Results
{
    public interface IMultipleCloseFileResult
    {
        bool HasClosedAll { get; }
        EDialogResult DialogResult { get; }
        bool HasSavedAny { get; }
    }
}

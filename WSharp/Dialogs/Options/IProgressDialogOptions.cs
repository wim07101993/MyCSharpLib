namespace WSharp.Dialogs.Options
{
    public interface IProgressDialogOptions
    {
        bool ShowOnForeground { get; }
        bool CanUserCancel { get; }
        bool MustUserCloseDialog { get; }
        bool IsInditerminate { get; }
        double Progress { get; }
        double Minimum { get; }
        double Maximum { get; }
        string Title { get; }
        string Message { get; }
    }
}

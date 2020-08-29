namespace WSharp.Dialogs.Results
{
    public interface ISaveFileDialogResult
    {
        EDialogResult Result { get; }
        string Path { get; }
    }
}

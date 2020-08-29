namespace WSharp.Dialogs.Results
{
    public interface IOpenFileDialogResult
    {
        string[] Paths { get; }
        FileFilter SelectedFileFilter { get; }
        EDialogResult Result { get; }
    }
}

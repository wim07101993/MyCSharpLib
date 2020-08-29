namespace WSharp.Dialogs.Options
{
    public interface ISimpleDialogOptions
    {
        EDialogResult PossibleResults { get; }
        string Title { get; }
        string Message { get; }
    }
}

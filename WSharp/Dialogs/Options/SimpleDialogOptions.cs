namespace WSharp.Dialogs.Options
{
    public struct SimpleDialogOptions : ISimpleDialogOptions
    {
        public SimpleDialogOptions(EDialogResult possibleResults, string title, string message)
        {
            PossibleResults = possibleResults;
            Title = title;
            Message = message;
        }

        public EDialogResult PossibleResults { get; }

        public string Title { get; }

        public string Message { get; }
    }
}

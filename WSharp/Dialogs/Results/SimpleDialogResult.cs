namespace WSharp.Dialogs.Results
{
    public struct SimpleDialogResult : ISimpleDialogResult
    {
        public SimpleDialogResult(EDialogResult result)
        {
            Result = result;
        }

        public EDialogResult Result { get; }
    }
}

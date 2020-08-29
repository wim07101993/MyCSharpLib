namespace WSharp.Dialogs.Results
{
    public struct ProgressDialogResult : IProgressDialogResult
    {
        public ProgressDialogResult(EDialogResult result)
        {
            Result = result;
        }

        public EDialogResult Result { get; }
    }
}

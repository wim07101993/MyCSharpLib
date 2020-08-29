namespace WSharp.Dialogs.Results
{
    public struct SaveFileDialogResult : ISaveFileDialogResult
    {
        public SaveFileDialogResult(EDialogResult result, string path)
        {
            Result = result;
            Path = path;
        }

        public EDialogResult Result { get; }

        public string Path { get; }
    }
}

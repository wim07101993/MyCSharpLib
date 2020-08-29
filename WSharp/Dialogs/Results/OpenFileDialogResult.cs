namespace WSharp.Dialogs.Results
{
    public struct OpenFileDialogResult : IOpenFileDialogResult
    {
        public OpenFileDialogResult(string[] paths, FileFilter selectedFileFilter, EDialogResult result)
        {
            Paths = paths;
            SelectedFileFilter = selectedFileFilter;
            Result = result;
        }

        public string[] Paths { get; }

        public FileFilter SelectedFileFilter { get; }

        public EDialogResult Result { get; }
    }
}

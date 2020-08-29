namespace WSharp.Dialogs.Options
{
    public struct ProgressDialogOptions : IProgressDialogOptions
    {
        public ProgressDialogOptions(
            bool showOnForeground,
            bool canUserCancel,
            bool mustUserCloseDialog,
            bool isInditerminate,
            double progress,
            double minimum,
            double maximum,
            string title,
            string message)
        {
            ShowOnForeground = showOnForeground;
            CanUserCancel = canUserCancel;
            MustUserCloseDialog = mustUserCloseDialog;
            IsInditerminate = isInditerminate;
            Progress = progress;
            Minimum = minimum;
            Maximum = maximum;
            Title = title;
            Message = message;
        }

        public ProgressDialogOptions(
            bool showOnForeground,
            bool canUserCancel,
            bool mustUserCloseDialog,
            double progress,
            double minimum,
            double maximum,
            string title,
            string message)
        {
            ShowOnForeground = showOnForeground;
            CanUserCancel = canUserCancel;
            MustUserCloseDialog = mustUserCloseDialog;
            IsInditerminate = false;
            Progress = progress;
            Minimum = minimum;
            Maximum = maximum;
            Title = title;
            Message = message;
        }

        public ProgressDialogOptions(
            bool showOnForeground,
            bool canUserCancel,
            bool mustUserCloseDialog,
            bool isInditerminate,
            string title,
            string message)
        {
            ShowOnForeground = showOnForeground;
            CanUserCancel = canUserCancel;
            MustUserCloseDialog = mustUserCloseDialog;
            IsInditerminate = isInditerminate;
            Progress = double.NaN;
            Minimum = double.NaN;
            Maximum = double.NaN;
            Title = title;
            Message = message;
        }

        public bool ShowOnForeground { get; }

        public bool CanUserCancel { get; }

        public bool MustUserCloseDialog { get; }

        public bool IsInditerminate { get; }

        public double Progress { get; }

        public double Minimum { get; }

        public double Maximum { get; }

        public string Title { get; }

        public string Message { get; }
    }
}

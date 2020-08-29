using System;

namespace WSharp.Dialogs.Options
{
    public struct ErrorDialogOptions : IErrorDialogOptions
    {
        public ErrorDialogOptions(
            string title, string message, bool showException, Exception exception)
        {
            Title = title;
            Message = message;
            ShowException = showException;
            Exception = exception;
        }

        public string Title { get; }

        public string Message { get; }

        public bool ShowException { get; }

        public Exception Exception { get; }
    }
}

using System;

namespace WSharp.Dialogs.Options
{
    public interface IErrorDialogOptions
    {
        string Title { get; }
        string Message { get; }
        bool ShowException { get; }
        Exception Exception { get; }
    }
}

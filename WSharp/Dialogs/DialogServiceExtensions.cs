using System;
using System.Threading.Tasks;

using WSharp.Dialogs.Options;
using WSharp.Dialogs.Results;

namespace WSharp.Dialogs
{
    public static class DialogServiceExtensions
    {
        public static IErrorDialogResult ShowErrorDialog(this IDialogService service,
            string title, Exception e, string message, bool showException)
           => service.ShowErrorDialog(new ErrorDialogOptions(
               title, message ?? e.Message, showException, e));

        public static Task<IErrorDialogResult> ShowErrorDialogAsync(this IDialogService service, 
            string title, Exception e, string message, bool showException)
            => service.ShowErrorDialogAsync(new ErrorDialogOptions(
                title, message ?? e.Message, showException, e));
    }
}

using System;
using System.Threading.Tasks;

using WSharp.Dialogs.Options;
using WSharp.Dialogs.Results;

namespace WSharp.Dialogs
{
    public interface IDialogService
    {
        IErrorDialogResult ShowErrorDialog(IErrorDialogOptions options);

        IOpenFileDialogResult ShowOpenFileDialog(IOpenFileDialogOptions options);

        IProgressDialogResult ShowProgressDialog(IProgressDialogOptions options);

        ISaveFileDialogResult ShowSaveFileDialog(ISaveFileDialogOptions options);

        ISimpleDialogResult ShowSimpleDialog(ISimpleDialogOptions options);

        Task<ISaveFileDialogResult> ShowSaveFileDialogAsync(ISaveFileDialogOptions options);

        Task<IProgressDialogResult> ShowProgressDialogAsync(IProgressDialogOptions options);

        Task<IOpenFileDialogResult> ShowOpenFileDialogAsync(IOpenFileDialogOptions options);

        Task<IErrorDialogResult> ShowErrorDialogAsync(IErrorDialogOptions options);

        Task<ISimpleDialogResult> ShowSimpleDialogAsync(ISimpleDialogOptions options);
    }
}

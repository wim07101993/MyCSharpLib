using System.Threading.Tasks;

using WSharp.Dialogs.Options;
using WSharp.Dialogs.Results;

namespace WSharp.Dialogs
{
    public interface IDialogService
    {
        IErrorDialogResult ShowErrorDialog(IErrorDialogOptions options);

        Task<IErrorDialogResult> ShowErrorDialogAsync(IErrorDialogOptions options);

        IOpenFileDialogResult ShowOpenFileDialog(IOpenFileDialogOptions options);

        Task<IOpenFileDialogResult> ShowOpenFileDialogAsync(IOpenFileDialogOptions options);

        IProgressDialogResult ShowProgressDialog(IProgressDialogOptions options);

        Task<IProgressDialogResult> ShowProgressDialogAsync(IProgressDialogOptions options);

        ISaveFileDialogResult ShowSaveFileDialog(ISaveFileDialogOptions options);

        Task<ISaveFileDialogResult> ShowSaveFileDialogAsync(ISaveFileDialogOptions options);

        ISaveUnsavedFilesDialogResult ShowSaveUnsavedFilesDialog(ISaveUnsavedFilesDialogOptions options);

        Task<ISaveUnsavedFilesDialogResult> ShowSaveUnsavedFilesDialogAsync(ISaveUnsavedFilesDialogOptions options);

        ISimpleDialogResult ShowSimpleDialog(ISimpleDialogOptions options);

        Task<ISimpleDialogResult> ShowSimpleDialogAsync(ISimpleDialogOptions options);


    }
}

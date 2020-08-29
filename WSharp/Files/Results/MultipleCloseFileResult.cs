using WSharp.Dialogs;

namespace WSharp.Files.Results
{
    public struct MultipleCloseFileResult : IMultipleCloseFileResult
    {
        public MultipleCloseFileResult(EDialogResult dialogResult, bool hasSavedAny)
        {
            DialogResult = dialogResult;
            HasSavedAny = hasSavedAny;
        }

        public bool HasClosedAll
        {
            get
            {
                switch (DialogResult)
                {
                    case EDialogResult.None:
                    case EDialogResult.Ok:
                        return true;

                    default: return false;
                }
            }
        }

        public EDialogResult DialogResult { get; }

        public bool HasSavedAny { get; }
    }
}

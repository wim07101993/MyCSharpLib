using System;

namespace WSharp.Dialogs
{
    [Flags]
    public enum EDialogResult
    {
        None = 0b0000_0000,
        Ok = 0b0000_0001,
        Cancel = 0b0000_0010,
        Yes = 0b0000_0100,
        No = 0b0000_1000,
        Close = 0b0001_0000,
    }
}

using System.ComponentModel;

namespace MyCSharpLib.Services
{
    /// <summary>
    /// Interface to determine what kind of strings are needed in the application.
    /// </summary>
    public interface IStrings : INotifyPropertyChanged
    {
        string Title { get; set; }
        string WrongStringsTypeExceptionMessage { get; set; }
    }
}

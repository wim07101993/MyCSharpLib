using Prism.Mvvm;

namespace MyCSharpLib.Services
{
    /// <summary>
    /// Mode that holds all the string values of the application. It is saved and loaded via a json file of which
    /// the path is stored in the settings file.
    /// Extends from the <see cref="BindableBase"/>.
    /// </summary>
    public class Strings : BindableBase, IStrings
    {
        #region FIELDS

        private string _title = "Line controller";
        private string _wrongStringsTypeExceptionMessage = "Cannot handle strings of incorrect type";

        #endregion FIELDS


        #region PROPERTIES

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public string WrongStringsTypeExceptionMessage
        {
            get => _wrongStringsTypeExceptionMessage;
            set => SetProperty(ref _wrongStringsTypeExceptionMessage, value);
        }

        #endregion PROPERTIES
    }
}

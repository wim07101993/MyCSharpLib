using Prism.Mvvm;

namespace MyCSharpLib.Services
{
    public class StringsSettings : BindableBase, IStringsSettings
    {
        private string _language;

        public string Language
        {
            get => _language;
            set => SetProperty(ref _language, value);
        }
    }
}

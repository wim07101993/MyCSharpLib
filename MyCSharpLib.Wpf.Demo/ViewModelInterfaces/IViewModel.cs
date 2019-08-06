using MyCSharpLib.Wpf.Demo.Strings;
using System.ComponentModel;

namespace MyCSharpLib.Wpf.Demo.ViewModelInterfaces
{
    public interface IViewModel : INotifyPropertyChanged
    {
        ApplicationStrings Strings { get; }
    }
}

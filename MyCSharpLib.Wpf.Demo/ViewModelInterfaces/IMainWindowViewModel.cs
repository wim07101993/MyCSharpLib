namespace MyCSharpLib.Wpf.Demo.ViewModelInterfaces
{
    public interface IMainWindowViewModel : IViewModel
    {
        ILoggingViewModel LoggingViewModel { get; }
    }
}

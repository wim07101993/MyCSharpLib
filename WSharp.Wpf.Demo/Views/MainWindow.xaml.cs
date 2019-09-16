using WSharp.Wpf.Demo.ViewModelInterfaces;

namespace WSharp.Wpf.Controls.Demo.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow(IMainWindowViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
        }

        public IMainWindowViewModel ViewModel
        {
            get => DataContext as IMainWindowViewModel;
            set => DataContext = value;
        }
    }
}

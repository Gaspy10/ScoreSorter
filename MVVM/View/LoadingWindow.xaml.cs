using System.Windows;
using MVVM.ViewModel;

namespace MVVM.View
{
    /// <summary>
    /// Interaction logic for LoadingWindow.xaml
    /// </summary>
    public partial class LoadingWindow : Window
    {
        internal LoadingWindow(LoadingWindowViewModel dc)
        {
            InitializeComponent();

            DataContext = dc;
        }
    }
}

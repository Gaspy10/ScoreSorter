using System.Windows;
using MVVM.ViewModel;

namespace MVVM.View
{
    /// <summary>
    /// Interaction logic for ErrorWindow.xaml
    /// </summary>
    public partial class ErrorWindow : Window
    {
        internal ErrorWindow(ErrorWindowViewModel evm)
        {
            InitializeComponent();

            DataContext = evm;
        }
    }
}

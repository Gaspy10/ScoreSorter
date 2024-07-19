using MVVM.ViewModel;
using System.Windows;

namespace MVVM.View
{
    /// <summary>
    /// Interaction logic for EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        public EditWindow(SecondWindowViewModel vm)
        {
            InitializeComponent();

            DataContext = vm;
        }
    }
}

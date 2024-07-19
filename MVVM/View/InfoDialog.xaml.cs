using System.Windows;
using MVVM.ViewModel;

namespace MVVM.View
{
    /// <summary>
    /// Interaction logic for InfoDialog.xaml
    /// </summary>
    public partial class InfoDialog : Window
    {
        public InfoDialog(InfoDialogViewModel ivm)
        {
            InitializeComponent();

            DataContext = ivm;
        }
    }
}

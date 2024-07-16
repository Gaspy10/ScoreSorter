using System.Windows.Controls;
using MVVM.ViewModel;

namespace MVVM.CustomUserControls
{
    /// <summary>
    /// Interaction logic for WindowInteract.xaml
    /// </summary>
    public partial class WindowInteract : UserControl
    {
        public WindowInteract()
        {
            InitializeComponent();

            DataContext = new WindowInteraction();
        }
    }
}

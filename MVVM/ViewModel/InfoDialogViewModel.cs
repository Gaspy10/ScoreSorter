using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MVVM.ViewModel
{
    public class InfoDialogViewModel : ViewModelBase
    {
        public bool ContinueState { get; set; } = false;

        public RelayCommand ContinueCommand => new RelayCommand(execute => ClickedContinue(execute), canExecute => true);
        public RelayCommand CancelCommand => new RelayCommand(execute => ClickedCancel(execute), canExecute => true);
        public RelayCommand DragCommand => new RelayCommand(execute => DragWindow(execute), canExecute => ConditionalFunctions.CanInteract(canExecute));

        private void ClickedContinue(object parameter)
        {
            Window window = (Window)parameter;

            window.Close();

            ContinueState = true;
        }

        private void ClickedCancel(object parameter)
        {
            Window window = (Window)parameter;

            window.Close();
        }
    }
}

using MVVM.Model;
using System.ComponentModel;

namespace MVVM.ViewModel
{
    internal class LoadingWindowViewModel : ViewModelBase
    {
        private readonly ProcessStateProperty psp;
        public string ProcessStateText
        {
            get { return psp.ProcessState; }
            set { psp.ProcessState = value; }
        }

        public LoadingWindowViewModel(ProcessStateProperty processStateProperty)
        {
            psp = processStateProperty;
            psp.PropertyChanged += ProcessStatePropertyChanged;
        }

        private void ProcessStatePropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(ProcessStateText));
        }

        public RelayCommand DragCommand => new RelayCommand(execute => DragWindow(execute), canExecute => ConditionalFunctions.CanInteract(canExecute));
    }
}

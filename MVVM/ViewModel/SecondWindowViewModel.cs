using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace MVVM.ViewModel
{
    public class SecondWindowViewModel : ViewModelBase
    {
        public RelayCommand OkCommand => new RelayCommand(execute => OkClick(execute), canExecute => ConditionalFunctions.CanAdd(PointsInput));
        public RelayCommand CancelCommand => new RelayCommand(execute => OkClick(execute), canExecute => true);
        public RelayCommand DragCommand => new RelayCommand(execute => DragWindow(execute), canExecute => ConditionalFunctions.CanInteract(canExecute));

        public bool DataChanged { get; set; }

        private string nameInput;
        private string pointsInput;
        public string NameInput
        {
            get { return nameInput; }
            set { nameInput = value; OnPropertyChanged(); }
        }

        public string PointsInput
        {
            get { return pointsInput; }
            set { pointsInput = value; OnPropertyChanged(); }
        }

        private void OkClick(object parameter)
        {
            Window window = (Window)parameter;
               
            window.Close();

            DataChanged = true;
        }

        public void SetInitialValues(string name)
        {
            NameInput = name;
        }

        internal void DragWindow(object parameter)
        {
            Window window = (Window)parameter;
            window.DragMove();
        }
    }
}

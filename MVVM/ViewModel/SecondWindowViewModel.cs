using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MVVM.ViewModel
{
    public class SecondWindowViewModel : ViewModelBase
    {
        ConditionalFunctions cf = new ConditionalFunctions();

        public RelayCommand OkCommand => new RelayCommand(execute => OkClick(execute), canExecute => true);
        public RelayCommand CancelCommand => new RelayCommand(execute => OkClick(execute), canExecute => true);

        private string nameInput;
        private string pointsInput;

        public string NameInput {
            get {  return nameInput; } 
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
        }

        public void SetInitialValues(string name)
        {
            NameInput = name;
        }
    }
}

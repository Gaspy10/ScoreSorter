using System.Diagnostics;
using System.IO;
using System.Windows;

namespace MVVM.ViewModel
{
    internal class SuccesWindowViewModel : ViewModelBase
    {
        public RelayCommand DragCommand => new RelayCommand(execute => DragWindow(execute), canExecute => ConditionalFunctions.CanInteract(canExecute));
        public RelayCommand OkCommand => new RelayCommand(execute => CloseWindow(execute), canExecute => true);

        public string FolderPath { get; set; }

        public RelayCommand OpenFileCommand => new RelayCommand(execute => OpenExplorer(execute), canExecute => true);
        private void OpenExplorer(object parameter)
        {
            CloseWindow(parameter);
            string directoryPath = Directory.Exists(FolderPath) ? FolderPath : Path.GetDirectoryName(FolderPath);

            Process.Start(new ProcessStartInfo
            {
                FileName = "explorer.exe",
                Arguments = directoryPath,
                UseShellExecute = true
            });
        }
        internal void CloseWindow(object parameter)
        {
            Window window = (Window)parameter;
            window.Close();
        }

        private string succesMessage;
        public string SuccesMessage 
        {
            get { return (succesMessage + " succesfully."); }
            set { succesMessage = value; OnPropertyChanged(); }
        }
    }
}

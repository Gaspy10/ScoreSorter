using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MVVM.ViewModel
{
    internal class WindowInteraction
    {
        ConditionalFunctions cf = new ConditionalFunctions();

        public RelayCommand CloseCommand => new RelayCommand(execute => CloseWindow(execute), canExecute => cf.CanInteract(canExecute));
        public RelayCommand ResizeCommand => new RelayCommand(execute => ResizeWindow(execute), canExecute => cf.CanInteract(canExecute));
        public RelayCommand MinimizeCommand => new RelayCommand(execute => MinimizeWindow(execute), canExecute => cf.CanInteract(canExecute));
        
        internal void CloseWindow(object parameter)
        {
            Window window = (Window)parameter;
            window.Close();
        }
        internal void ResizeWindow(object parameter)
        {
            Window window = (Window)parameter;
            if (window.WindowState == WindowState.Maximized)
            {
                window.WindowState = WindowState.Normal;
            }
            else
            {
                window.WindowState = WindowState.Maximized;
            }
        }
        internal void MinimizeWindow(object parameter)
        {
            Window window = (Window)parameter;
            window.WindowState = WindowState.Minimized;
        }
    }
}

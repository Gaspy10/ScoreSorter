using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Media.Animation;

namespace MVVM.ViewModel
{
    internal class WindowInteraction
    {
        public RelayCommand CloseCommand => new RelayCommand(execute => CloseWindow(execute), canExecute => ConditionalFunctions.CanInteract(canExecute));
        public RelayCommand ResizeCommand => new RelayCommand(execute => ResizeWindow(execute), canExecute => ConditionalFunctions.CanInteract(canExecute));
        public RelayCommand MinimizeCommand => new RelayCommand(execute => MinimizeWindow(execute), canExecute => ConditionalFunctions.CanInteract(canExecute));
        
        internal void CloseWindow(object parameter)
        {
            Window window = (Window)parameter;
            Storyboard storyboard = new Storyboard();

            DoubleAnimation closeAnimation = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = new Duration(TimeSpan.FromSeconds(0.2))
            };

            Storyboard.SetTarget(closeAnimation, window);
            Storyboard.SetTargetProperty(closeAnimation, new PropertyPath(Window.OpacityProperty));

            // Add the animation to the storyboard
            storyboard.Children.Add(closeAnimation);

            // Attach the Completed event to the storyboard (not the animation)
            storyboard.Completed += (s, e) =>
            {
                window.Close();
            };

            // Begin the storyboard
            storyboard.Begin();

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

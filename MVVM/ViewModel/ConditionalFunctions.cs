using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MVVM.ViewModel
{
    internal class ConditionalFunctions
    {
        internal static bool CanAdd(string points)
        {
            return int.TryParse(points, out _);
        }

        internal static bool CanInteract(object parameter)
        {
            if (parameter is Window)
            {
                Window window = (Window)parameter;
                if (window != null)
                    return true;
            }
            return false;
        }

    }
}

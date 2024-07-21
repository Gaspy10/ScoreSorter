using System.Collections.ObjectModel;
using System.Windows;
using MVVM.Model;

namespace MVVM.ViewModel
{
    internal class ConditionalFunctions
    {
        internal static bool CanAdd(string points)
        {
            if(int.TryParse(points, out int parsedPoints))
            {
                return parsedPoints > 0;
            }
            return false;
            //return int.TryParse(points, out _);
        }

        internal static bool CanExport(ObservableCollection<Item> items)
        {
            if(items.Count > 0)
            {
                return true;
            }
            return false;
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MVVM.ViewModel
{
    class ConditionalFunctions
    {
        public bool InstanceMember { get; set; }

        internal bool CanAdd(string points)
        {

            if (string.IsNullOrEmpty(points))
                return false;

            try
            {
                int result = int.Parse(points);
                return true;
            }
            catch (Exception ex) when (ex is FormatException || ex is OverflowException)
            {
                return false;
            }
        }

        internal bool CanInteract(object parameter)
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

using MVVM.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM.ViewModel
{
    internal class OpenErrorWindow
    {
        private LoadingWindow AlmostClosedWindow { get; set; }
        public OpenErrorWindow(LoadingWindow lw)
        {
            AlmostClosedWindow = lw;
        }
        internal void ThrowError(string error)
        {
            ErrorWindowViewModel evm = new ErrorWindowViewModel("Something went wrong: " + error + " Please make sure the selected image is in correct format.");
            ErrorWindow errorWindow = new ErrorWindow(evm);
            AlmostClosedWindow.Close();
            errorWindow.ShowDialog();
        }
    }
}

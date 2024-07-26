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
            ErrorWindowViewModel evm = new ErrorWindowViewModel(error);
            ErrorWindow errorWindow = new ErrorWindow(evm);
            AlmostClosedWindow.Close();
            errorWindow.ShowDialog();
        }
    }
}

using MVVM.ViewModel;

namespace MVVM.Model
{
    internal class ProcessStateProperty : ViewModelBase
    {
        private string? processState;
        public string ProcessState 
        {
            get { return processState; }
            set
            {
                processState = value;
                OnPropertyChanged(nameof(ProcessState));
            }
        }
    }
}

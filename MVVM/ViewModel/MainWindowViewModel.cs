using MVVM.Model;
using MVVM.View;
using MVVM.ViewModel.Reorder_function;
using System.Collections.ObjectModel;
using System.Windows;
using MVVM.Python;
using System.IO;
using System.Text.Json;
//using WinForms = System.Windows.Forms;

namespace MVVM.ViewModel
{
    internal class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<Item> Items { get; set; }

        private RelayCommand addCommand;
        public RelayCommand AddCommand => addCommand ??= new RelayCommand(execute => AddItem(), canExecute => CanAdd);
        public RelayCommand RemoveCommand => new RelayCommand(execute => RemoveItem(), canExecute => SelectedItem != null);
        public RelayCommand ShuffleCommand => new RelayCommand(execute => ShuffleCollection());
        public RelayCommand SortCommand => new RelayCommand(execute => SortCollection());
        public RelayCommand EditCommand => new RelayCommand(execute => EditItem(), canExecute => CanEdit(canExecute));
        public RelayCommand DragCommand => new RelayCommand(execute => DragWindow(execute), canExecute => ConditionalFunctions.CanInteract(canExecute));
        public RelayCommand ImportCommand => new RelayCommand(execute => ImportFilePath(), canExecute => true);

        private async void ImportFilePath()
        {
            InfoDialogViewModel ivm = new InfoDialogViewModel();
            InfoDialog id = new InfoDialog(ivm);
            id.ShowDialog();

            if (!ivm.ContinueState)
                return;

            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Image Files (*.jpg;*.jpeg;*.png;)|*.jpg;*.jpeg;*.png;",
                FilterIndex = 1,
                Multiselect = false
            };

            bool? result = dialog.ShowDialog();
            if (result != true)
            {
                return;
            }
            string FilePath = dialog.FileName;

            ProcessStateProperty imageProcess = new ProcessStateProperty();
            LoadingWindow loadingWindow = new LoadingWindow(imageProcess);

            Progress<ProcessStateProperty> progress = new Progress<ProcessStateProperty>();
            loadingWindow.Show();

            progress.ProgressChanged += (sender, e) =>
            {
                imageProcess.ProcessState = e.ProcessState;
            };

            List<Student> students = await Task.Run(() => StartLoadingWindowAsync(progress, FilePath));

            Items.Clear();

            for(int i = 0; i < students.Count; i++)
            {
                Items.Add(new Item { Name = students[i].name, 
                Points = int.Parse(students[i].points) });
            }

            imageProcess.ProcessState = "Done!";

            Items.Sort();
        }

        internal class Student
        {
            public string name { get; set; }
            public string points { get; set; }
        }

        private List<Student>StartLoadingWindowAsync(IProgress<ProcessStateProperty> progress, string FilePath)
        {
            ProcessStateProperty report = new ProcessStateProperty();

            report.ProcessState = "Preprocessing the selected image...";
            progress.Report(report);
            string processedImagePath = PythonService.ProcessPhoto(FilePath);

            report.ProcessState = "Exporting to .json file...";
            progress.Report(report);
            string jsonPath = PythonService.ExportJson(processedImagePath);

            report.ProcessState = "Deserializing json...";
            progress.Report(report);
            List<Student> students = new List<Student> { };
            students = JsonSerializer.Deserialize<List<Student>>(File.ReadAllText(jsonPath));

            report.ProcessState = "Importing data to datagrid...";
            return students;
        }

        public RelayCommand ExportCommand => new RelayCommand(execute => Export(execute), canExecute => ConditionalFunctions.CanExport(Items));

        private void Export(object execute)
        {
            throw new NotImplementedException();
        }
        public MainWindowViewModel()
        {
            Items = new ObservableCollection<Item>();

            Items.Add(new Item { Name = "John", Points = 10 });
        }

        private bool canAdd = false;
        public bool CanAdd
        {
            get { return canAdd; }
            set
            {
                if (canAdd != value)
                {
                    canAdd = value;
                    addCommand?.RaiseCanExecuteChanged();
                    OnPropertyChanged();
                }
            }
        }

        private string inputtedName;
        private string inputtedPoints;

        public string InputtedName
        {
            get { return inputtedName; }
            set { inputtedName = value; OnPropertyChanged(nameof(InputtedName)); }
        }

        public string InputtedPoints
        {
            get { return inputtedPoints; }
            set { inputtedPoints = value; OnPropertyChanged(nameof(InputtedPoints)); }
        }

        private string newName;
        public string NewName
        {
            get { return newName; }
            set
            {
                newName = value;
                OnPropertyChanged(nameof(NewName));
            }
        }

        private string newSurname;
        public string NewSurname
        {
            get { return newSurname; }
            set
            {
                newSurname = value;
                CanAdd = ConditionalFunctions.CanAdd(newSurname);
                OnPropertyChanged(nameof(NewSurname));
            }
        }

        private Item selectedItem;
        public Item SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        //function definitions
        private void AddItem()
        {
            Items.Add(new Item
            {
                Name = newName,
                Points = int.Parse(newSurname),
            });

            NewName = "";
            NewSurname = "";
            Items.Sort();
        }

        private void RemoveItem()
        {
            Items.Remove(selectedItem);
        }

        private void ShuffleCollection()
        {
            Items.Shuffle();
        }

        private void SortCollection()
        {
            Items.Sort();
        }

        private void EditItem()
        {
            SecondWindowViewModel swm = new SecondWindowViewModel()
            {
                NameInput = SelectedItem.Name,
                PointsInput = SelectedItem.Points.ToString(),
            };

            EditWindow ew = new EditWindow(swm);

            ew.ShowDialog();
            if (!swm.DataChanged)
                return;

            var input = (SecondWindowViewModel)ew.DataContext;

            selectedItem.Name = input.NameInput;
            selectedItem.Points = int.Parse(input.PointsInput);

            Items.Sort();
        }

        private bool CanEdit(object parameter)
        {
            if (selectedItem != null)
            {
                return true;
            }
            return false;
        }

        internal void DragWindow(object parameter)
        {
            Window window = (Window)parameter;
            window.DragMove();
        }
    }
}

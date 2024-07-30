using Microsoft.Win32;
using MVVM.Model;
using MVVM.Python;
using MVVM.View;
using MVVM.ViewModel.Reorder_function;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows;
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
        public RelayCommand SaveCommand => new RelayCommand(execute => Save(), canExecute => ConditionalFunctions.CanExport(Items));
        public RelayCommand ClearCommand => new RelayCommand(execute => Clear(), canExecute => ConditionalFunctions.CanExport(Items));

        private void Clear()
        {
            Items.Clear();
        }

        private string SavedPath { get; set; }

        private void Save()
        {
            if(SavedPath == null)
            {
                SaveAs();
                return;
            }
            string json = JsonSerializer.Serialize(Items);
            File.WriteAllText(SavedPath, json);
        }

        public RelayCommand SaveAsCommand => new RelayCommand(execute => SaveAs(), canExecute => ConditionalFunctions.CanExport(Items));

        private void SaveAs()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Json files (*.json)|*.json",
                FilterIndex = 1,
                RestoreDirectory = true
            };

            if (saveFileDialog.ShowDialog() != true)
            {
                return;
            }

            SavedPath = saveFileDialog.FileName;

            Save();
        }

        public RelayCommand OpenCommand => new RelayCommand(execute => Open(), canExecute => true);

        private void Open()
        {
            List<Item> items = new List<Item>();

            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Json files (*.json)|*.json",
                FilterIndex = 1,
                RestoreDirectory = true
            };

            if (openFileDialog.ShowDialog() != true)
            {
                return;
            }

            string json = File.ReadAllText(openFileDialog.FileName);
            items = JsonSerializer.Deserialize<List<Item>>(json);

            for (int i = 0; i < items.Count; i++)
            {
                Items.Add(new Item { Name = items[i].Name, Points = items[i].Points });
            }
            Items.Sort();
        }

        private async Task ImportFilePath()
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
            LoadingWindowViewModel lwm = new LoadingWindowViewModel(imageProcess);
            LoadingWindow loadingWindow = new LoadingWindow(lwm);

            OpenErrorWindow errorWindowOperator = new OpenErrorWindow(loadingWindow);

            Progress<ProcessStateProperty> progress = new Progress<ProcessStateProperty>();
            loadingWindow.Show();

            progress.ProgressChanged += (sender, e) =>
            {
                imageProcess.ProcessState = e.ProcessState;
            };

            List<Student> students;

            try
            {
                students = await Task.Run(() => StartLoadingWindowAsync(progress, FilePath));
            }
            catch (Exception e)
            {
                errorWindowOperator.ThrowError(e.Message);
                return;
            }

            ObservableCollection<Item> OriginalItems = new ObservableCollection<Item>(Items.Select(item => (Item)item.Clone()));
            Items.Clear();

            for(int i = 0; i < students.Count; i++)
            {
                Items.Add(new Item { Name = students[i].Name, 
                Points = int.Parse(students[i].Points) });
            }

            imageProcess.ProcessState = "Sorting the collection";

            try
            {
                Items.Sort();
            }
            catch (Exception e)
            {
                Items = new ObservableCollection<Item>(OriginalItems.Select(item => (Item)item.Clone()));
                OnPropertyChanged(nameof(Items));
                Items.Sort();
                errorWindowOperator.ThrowError(e.Message);
                return;
            }
            imageProcess.ProcessState = "Done!";
        }

        private void OpenNewErrorWindow(string error)
        {
            ErrorWindowViewModel evm = new ErrorWindowViewModel(error);
            ErrorWindow errorWindow = new ErrorWindow(evm);
            errorWindow.ShowDialog();
        }

        internal class Student
        {
            public string Name { get; set; }
            public string Points { get; set; }
            public string Order { get; set; }
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
            progress.Report(report);
            return students;
        }

        public RelayCommand ExportCommand => new RelayCommand(execute => Export(execute), canExecute => ConditionalFunctions.CanExport(Items));

        private void Export(object execute)
        {
            SaveFileDialog openFolder = new SaveFileDialog
            {
                Filter = "Png files (*.png)|*.png",
                FilterIndex = 1,
                RestoreDirectory = true
            };

            if(openFolder.ShowDialog() != true)
            {
                return;
            }

            string result = openFolder.FileName;

            Font font = new Font("Arial", 20);

            int lineHeight = (int)font.GetHeight() + 5;

            //calculate the width
            //int width = 1000;
            int height = Items.Count * lineHeight;

            int maxLength = -1;

            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].Name == null)
                {
                    continue;
                }
                if (Items[i].Name.Length > maxLength)
                {
                    maxLength = Items[i].Name.Length;
                }
            }

            float nameWidth = maxLength * 13.5f;
            int width = (int)(nameWidth) + 195;
            List<float> verticalSeparators = new List<float> { 100, 120 + nameWidth };

            using (Bitmap bitmap = new Bitmap(width, height))
            {
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    graphics.Clear(Color.White);

                    for (int i = 0; i < Items.Count; i++)
                    {
                        graphics.DrawString(Items[i].Order, font, Brushes.Black, 4, i * lineHeight);
                        //graphics.DrawString("|", font, Brushes.Black, 95, i * lineHeight);
                        graphics.DrawString(Items[i].Name, font, Brushes.Black, 110, i * lineHeight);
                        //graphics.DrawString("|", font, Brushes.Black, 115+nameWidth, i * lineHeight);
                        graphics.DrawString(Items[i].Points.ToString(), font, Brushes.Black, nameWidth+130, i * lineHeight);

                        System.Drawing.Point horizonatalStart = new System.Drawing.Point(0, (i + 1) * lineHeight);
                        System.Drawing.Point horizontalEnd = new System.Drawing.Point(width, (i + 1) * lineHeight);

                        graphics.DrawLine(Pens.Black, horizonatalStart, horizontalEnd);
                    }
                    foreach (float verticalSeparator in verticalSeparators)
                    {
                        System.Drawing.Point verticalStart = new System.Drawing.Point((int)verticalSeparator, 0);
                        System.Drawing.Point verticalEnd = new System.Drawing.Point((int)verticalSeparator, height);

                        graphics.DrawLine(Pens.Black, verticalStart, verticalEnd);
                    }
                }
                bitmap.Save(result, ImageFormat.Png);
            }
            ShowSucces("Image exported", openFolder.FileName);
        }

        internal void ShowSucces(string msg, string path)
        {
            SuccesWindowViewModel swvm = new SuccesWindowViewModel();
            swvm.SuccesMessage = msg;
            swvm.FolderPath = path;
            SuccesWindow sw = new SuccesWindow(swvm);
            sw.ShowDialog();
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
    }
}

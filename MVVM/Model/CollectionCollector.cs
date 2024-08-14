using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MVVM.ViewModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM.Model
{
    class CollectionCollector
    {
        MainWindowViewModel mwvm;
        public ObservableCollection<ObservableCollection<Item>> ItemsCollection { get; set; }
        public ObservableCollection<Item> CurrentCollection { get; set; }
        public CollectionCollector(MainWindowViewModel argument)
        {
            CurrentCollection = new ObservableCollection<Item>();
            CurrentCollection.Add(new Item { Name = "ASDF", Points = 964 });
            mwvm = argument;
            ItemsCollection = new ObservableCollection<ObservableCollection<Item>>();
            mwvm.OnCollectionChanged += CollectionCollector_OnCollectionChanged;
        }

        private void CollectionCollector_OnCollectionChanged(object sender, MainWindowViewModel.OnCollectionChangedEventArgs e)
        {
            ItemsCollection.Add(e.CollectedItems);
            mwvm.DepthIndex = ItemsCollection.Count - 1;
            Console.WriteLine(ItemsCollection[0][0].Name);
        }
    }
}

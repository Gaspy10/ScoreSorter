using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MVVM.ViewModel;
using System.Collections;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace MVVM.Model
{
    class CollectionCollector
    {
        public Stack<ObservableCollection<Item>> undoStack = new Stack<ObservableCollection<Item>>();
        public Stack<ObservableCollection<Item>> redoStack = new Stack<ObservableCollection<Item>>();
        public CollectionCollector(MainWindowViewModel mwvm)
        {
            mwvm.OnCollectionChanged += CollectionCollector_OnCollectionChanged;
        }

        private void CollectionCollector_OnCollectionChanged(object sender, MainWindowViewModel.OnCollectionChangedEventArgs e)
        {
            var copiedCollection = DeepCopy(e.CollectedItems);
            undoStack = StackOnStack(undoStack, redoStack);
            undoStack.Push(copiedCollection);
        }

        private ObservableCollection<Item> DeepCopy(ObservableCollection<Item> original)
        {
            var newCollection = new ObservableCollection<Item>();

            foreach (var item in original)
            {
                var newItem = new Item
                {
                    Points = item.Points,
                    Name = item.Name,
                    Order = item.Order
                };

                newCollection.Add(newItem);
            }

            return newCollection;
        }

        private Stack<ObservableCollection<Item>> StackOnStack(Stack<ObservableCollection<Item>> stack1 //bottom
                                ,Stack<ObservableCollection<Item>> stack2) //top
        {
            while (stack2.Count > 0)
            {
                stack1.Push(stack2.Pop());
            }
            return stack1;
        }
    }
}

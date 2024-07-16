using System.Collections.ObjectModel;
using MVVM.Model;

namespace MVVM.ViewModel.Reorder_function
{
    public static class Sorter
    {
        private static int min = int.MaxValue;

        public static void Sort(this ObservableCollection<Item> collection)
        {
            int size = collection.Count;
            int max = -9999;

            //find max points
            for (int i = 0; i < size; i++)
            {
                if (collection[i].Points > max)
                    max = collection[i].Points;
            }

            int[] counter = new int[max+1];
            int[] array = new int[max+1];
            Item[][] twoDArray = new Item[max+1][];

            for (int i = 0; i < size; i++)
            {
                array[collection[i].Points] += 1;
            }

            for (int i = 0; i <= max; i++)
            {
                twoDArray[i] = new Item[array[i]];
            }

            for (int i = 0; i < size; i++)
            {
                twoDArray[collection[i].Points][counter[collection[i].Points]] = collection[i];
                counter[collection[i].Points] += 1;
            }

            int cursor = 0;

            for (int i = 0; i < max+1; i++)
            {
                for(int j = 0; j < array[i]; j++)
                {
                    collection[cursor] = twoDArray[i][j];
                    cursor++;
                }
            }

            //mirror the collection
            for (int i = 0; i < collection.Count/2; i++)
            {
                Item temp = collection[i];
                collection[i] = collection[collection.Count - i - 1];
                collection[collection.Count - i - 1] = temp;
            }

            Item oldItem = new Item();
            oldItem.Points = -1;

            //set the order
            for (int i = 0; i < collection.Count; i++)
            {
                /*if (oldItem.Points != collection[i].Points)
                {
                    order++;
                }

                collection[i].Order = order.ToString();
                collection[i].Order += ".";
                oldItem = collection[i];*/
                if (array[collection[i].Points] > 1)
                {
                    //int originalI = i;
                    int itemsCount = array[collection[i].Points];
                    for(int j = 0; j < itemsCount; j++)
                    {
                        collection[i + j].Order = (i + 1).ToString();
                        collection[i + j].Order += ".-";
                        collection[i + j].Order += (i + itemsCount).ToString();
                        collection[i + j].Order += ".";
                    }
                    i = i + itemsCount - 1;
                }

                else
                {
                    collection[i].Order = (i+1).ToString();
                    collection[i].Order += ".";
                }
            }
        }
    }    
}

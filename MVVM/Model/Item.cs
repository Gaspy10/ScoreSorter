using System.ComponentModel;
using System.Diagnostics;

namespace MVVM.Model
{
    public class Item : INotifyPropertyChanged, ICloneable
    {
        private string name;
        private int points;
        private string order;


        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public int Points
        {
            get { return points; }
            set
            {
                if (points != value)
                {
                    points = value;
                    OnPropertyChanged(nameof(points));
                }
            }
        }

        public string Order
        {
            get { return order; }
            set
            {
                if (order != value)
                {
                    order = value;
                    OnPropertyChanged(nameof(order));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public object Clone()
        {
            return new Item { Name = this.Name, Points = this.Points, Order = this.Order };
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

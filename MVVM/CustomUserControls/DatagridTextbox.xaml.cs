using MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MVVM.CustomUserControls
{
    /// <summary>
    /// Interaction logic for DatagridTextbox.xaml
    /// </summary>
    public partial class DatagridTextbox : UserControl
    {
        public static readonly DependencyProperty NameTextProperty = DependencyProperty.Register(
          nameof(NameText), typeof(string), typeof(DatagridTextbox),
          new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnPropertyChanged)));

        public static readonly DependencyProperty PointsTextProperty = DependencyProperty.Register(
          nameof(PointsText), typeof(string), typeof(DatagridTextbox),
          new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnPropertyChanged)));

        public string NameText
        {
            get { return (string)GetValue(NameTextProperty); }
            set 
            { 
                SetValue(NameTextProperty, value);
                addName.Text = value;
            }
        }

        public string PointsText
        {
            get { return (string)GetValue(PointsTextProperty); }
            set 
            { 
                SetValue(PointsTextProperty, value);
                addPoints.Text = value;
            }
        }

        public DatagridTextbox()
        {
            InitializeComponent();
            addName.TextChanged += TextChanged;
            addPoints.TextChanged += TextChanged;
        }
        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if(textBox == null)
            {
                return;
            }

            if (textBox.Name == nameof(addPoints))
            {
                PointsText = addPoints.Text;
            }
            else if (textBox.Name == nameof(addName))
            {
                NameText = addName.Text;
            }
        }
        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DatagridTextbox ctrl)
            {
                if(e.Property.Name == nameof(NameText))
                    ctrl.addName.Text = (string)e.NewValue;
                else if (e.Property.Name == nameof(PointsText))
                    ctrl.addPoints.Text = (string)e.NewValue;
            }
        }
    }
}

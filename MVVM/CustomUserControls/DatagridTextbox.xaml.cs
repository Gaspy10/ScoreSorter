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
        public static readonly DependencyProperty NameTextProperty =
            DependencyProperty.Register("NameText", typeof(string), typeof(DatagridTextbox), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty PointsTextProperty =
            DependencyProperty.Register("PointsText", typeof(string), typeof(DatagridTextbox), new PropertyMetadata(string.Empty));

        public string NameText
        {
            get { return (string)GetValue(NameTextProperty); }
            set { SetValue(NameTextProperty, value); }
        }

        public string PointsText
        {
            get { return (string)GetValue(PointsTextProperty); }
            set { SetValue(PointsTextProperty, value); }
        }

        public DatagridTextbox()
        {
            InitializeComponent();
        }
    }
}

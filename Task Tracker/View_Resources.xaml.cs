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
using System.Windows.Shapes;

namespace Task_Tracker
{
    /// <summary>
    /// Interaction logic for View_Resources.xaml
    /// </summary>
    public partial class View_Resources : Window
    {
        public View_Resources(List<string> users)
        {
            InitializeComponent();
            resources.ItemsSource = users;  
        }

        private void Close_View(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}

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
using AssemblyGame.ViewModel;

namespace AssemblyGame.View
{
    /// <summary>
    /// Interaction logic for NewGameNameWindow.xaml
    /// </summary>
    public partial class NewGameNameWindow : Window
    {
        public NewGameNameWindow()
        {
            InitializeComponent();
        }

        private void OpenGameWindow(object sender, RoutedEventArgs e)
        {
            MainWindow objMainWindow = new MainWindow();
            objMainWindow.CityNameWindow.Content = NameBox.Text;
            objMainWindow.DataContext = this.DataContext;
            this.Visibility = Visibility.Hidden;
            objMainWindow.Show();
            //objMainWindow.DataContext = ;
            this.Close();
        }

    }
}

using AssemblyGame.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace AssemblyGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string CityNameView;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BackToMenu(object sender, RoutedEventArgs e)
        {
            MenuWindow objMenuWindow = new MenuWindow();
            this.Visibility = Visibility.Hidden;
            objMenuWindow.Show();
            this.Close();
        }

        private void SaveGame(object sender, RoutedEventArgs e)
        {
            SaveGameWindow objSaveGameWindow = new SaveGameWindow();
            this.Visibility = Visibility.Hidden;
            objSaveGameWindow.Show();
            this.Close();
        }

        private void Funds_Click(object sender, RoutedEventArgs e)
        {
            Funds fundsWindow = new Funds();
            fundsWindow.Show();
        }

        private void MainWindow_Closing(object? sender, CancelEventArgs e)
        {
            //_mainWindow.Close();
            Application.Current.Shutdown();
        }

    }
}

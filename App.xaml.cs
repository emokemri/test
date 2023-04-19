using AssemblyGame.Model;
using AssemblyGame.ViewModel;
using AssemblyGame.View;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.ComponentModel;

namespace AssemblyGame
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private GameModel _gameModel = null!;
        private GameViewModel _gameViewModel = null!;
        private MenuWindow _view = null!;
        //private NewGameNameWindow _newGameNameWindow = null!;
        private MainWindow _mainWindow = null!;

        public App()
        {
            Startup += new StartupEventHandler(App_startUp);
        }

        private void App_startUp(object sender, StartupEventArgs e)
        {
            _gameModel = new GameModel();
            _gameModel.startNewGame();

            _gameViewModel = new GameViewModel(_gameModel);
            
            _gameViewModel.GameExit += new EventHandler(ViewModel_GameExit);
            _gameViewModel.TaxChange += new EventHandler(ViewModel_SetTax);
            _gameViewModel.SpeedChange += new EventHandler<ChangeEventArgs>(ViewModel_SpeedChange);
            _gameViewModel.BuildingChoice += new EventHandler<ChangeEventArgs>(ViewModel_BuildingChoice);
            _view = new MenuWindow();
            _view.DataContext = _gameViewModel;
            _view.Show();
            //_newGameNameWindow = new NewGameNameWindow();
            //_gameViewModel.CityName = _newGameNameWindow.NameBox.Text;
            //_mainWindow = new MainWindow();
            //_mainWindow.DataContext = _gameViewModel;
            //_mainWindow.Closing += new System.ComponentModel.CancelEventHandler(MainWindow_Closing);

        }

        private void ViewModel_BuildingChoice(object? sender, ChangeEventArgs e)
        {
            if(e.SpeedMode == "Residental")
            {
                _gameViewModel.BuildingArea = new Residental(new Position(0, 0));
            }
            else if(e.SpeedMode == "Service")
            {
                _gameViewModel.BuildingArea = new Service(new Position(0, 0));
            }
            else if(e.SpeedMode == "Industrial")
            {
                _gameViewModel.BuildingArea = new Industrial(new Position(0, 0));
            }
            else if( e.SpeedMode == "Police")
            {
                _gameViewModel.BuildingArea = new Police(new Position(0, 0));
            }
            else if(e.SpeedMode == "Stadium")
            {
                _gameViewModel.BuildingArea = new Stadium(new Position(0, 0));
            }
            else if(e.SpeedMode == "HighSchool")
            {
                _gameViewModel.BuildingArea = new Education(new Position(0, 0),EducationLevel.HighSchool);
            }
            else if(e.SpeedMode == "University")
            {
                _gameViewModel.BuildingArea = new Education(new Position(0, 0), EducationLevel.University);
            }
            else if(e.SpeedMode == "Forest")
            {
                _gameViewModel.BuildingArea = new Forest(new Position(0, 0));
            }
            else if(e.SpeedMode == "Road")
            {
                _gameViewModel.BuildingArea = new Road(new Position(0, 0));
            }
            else if(_gameViewModel.BuildingArea is not null &&  _gameViewModel.BuildingArea.Name == e.SpeedMode)
            {
                _gameViewModel.BuildingArea = null;
            }
        }

        private void ViewModel_SpeedChange(object? sender, ChangeEventArgs e)
        {
            if(e.SpeedMode == "Slow")
            {
                _gameModel.GameSpeed = Speed.Slow;
            }
            else if(e.SpeedMode == "Normal")
            {
                _gameModel.GameSpeed = Speed.Normal;
            }
            else if(e.SpeedMode == "Fast")
            {
                _gameModel.GameSpeed = Speed.Fast;
            }
            else if(e.SpeedMode == "Pause")
            {
                _gameModel.GameSpeed = Speed.Pause;
            }
        }

        private void ViewModel_SetTax(object? sender, EventArgs e)
        {
            _gameModel.Tax = _gameViewModel.Tax;
        }

        private void ViewModel_GameExit(object? sender, EventArgs e)
        {
            //Shutdown(); //bezárja egész alkalmazást
            _mainWindow.Close();
        }

    }
}

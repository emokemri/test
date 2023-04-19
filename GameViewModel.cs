using AssemblyGame.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace AssemblyGame.ViewModel
{
    public class GameViewModel : ViewModelBase
    {
        #region Fields
        private GameModel _model;

        private string _currentTime;
        private int _year, _month, _day;
        private DispatcherTimer dt;
        private Stopwatch sw;

        public string cityName;
        private int _wealth;
        private IArea buildingArea;
        public Int32 Width => 30;
        public Int32 Height => 30;

        #endregion

        #region Properties

        public IArea BuildingArea
        {
            get => buildingArea;
            set
            {
                if(buildingArea != value)
                {
                    buildingArea = value;
                    OnPropertyChanged();
                }
            }
        }
        public string CityName
        {
            get { return cityName; }
            set
            {
                if (cityName != value)
                {
                    cityName = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Wealth
        {
            get { return _wealth; }
            set
            {
                if (_wealth != value)
                {
                    _wealth = value;
                    OnPropertyChanged();
                }
            }
        } 
        public string CurrentTime
        {
            get { return string.Format("{0:0000}.{1:00}.{2:00}", _year, _month, _day); }
            set
            {
                if (_currentTime != value)
                {
                    _currentTime = value;
                    _year = int.Parse(_currentTime.Split('.')[0]);
                    _month = int.Parse(_currentTime.Split('.')[1]);
                    _day = int.Parse(_currentTime.Split('.')[2]);
                }

                OnPropertyChanged("CurrentTime");
            }
        }
        public DelegateCommand NewGameCommand { get; private set; }
        public DelegateCommand ExitGameCommand { get; private set; }
        public DelegateCommand BuildingChoiceCommand { get; private set; }
        public DelegateCommand SpeedChangeCommand { get; private set; }
        public ObservableCollection<GameField> Fields { get; set; }

        #endregion

        #region Events

        public event EventHandler? GameExit;
        public event EventHandler<ChangeEventArgs>? SpeedChange;
        public event EventHandler<ChangeEventArgs>? BuildingChoice;

        #endregion

        #region Constructor
        public GameViewModel(GameModel model)
        {
            _model = model;
            _model.FieldChanged += new EventHandler<FieldChangedEventArgs>(Model_FieldChanged);
            sw = new Stopwatch();
            DispatcherTimerSetup();
            //_model.GameStarted += new EventHandler(Model_StartNewGame);
            Wealth = _model.Money;

            //NewGameCommand = new DelegateCommand(param => _model.startNewGame());
            ExitGameCommand = new DelegateCommand(param => OnGameExit());
            SpeedChangeCommand = new DelegateCommand(param => OnSpeedChange(param));
            BuildingChoiceCommand = new DelegateCommand(param => OnBuildingChoice(param));

            Fields = new ObservableCollection<GameField>();

            RefreshTable();
        }

        #endregion

        #region Private Methods

        private void DispatcherTimerSetup()
        {
            dt = new DispatcherTimer();
            dt.Interval = TimeSpan.FromSeconds(1);
            dt.Tick += new EventHandler(CurrentTimeText);
            DateTime currentDate = DateTime.Now;
            CurrentTime = string.Format("{0:0000}.{1:00}.{2:00}", currentDate.Year, currentDate.Month, currentDate.Day);
            dt.Start();
            sw.Start();
        }

        private void CurrentTimeText(object sender, EventArgs e)
        {
            //CurrentTime = DateTime.MinValue.ToString("HH:mm");
            if (sw.IsRunning)
            {
                // TimeSpan ts = 
                int seconds_passed = (int)sw.Elapsed.TotalSeconds;
                // CurrentTime = string.Format("{0:0000}.{1:00}.{2:00}", ts.Hours, ts.Minutes, ts.Seconds);
                DateTime startDate = new DateTime(_year, _month, _day);
                if (_model.GameSpeed == Speed.Fast)
                {
                    // fast mode 1 sec == 1 month
                    int monthsPassed = 1;
                    DateTime newDate = startDate.AddMonths(monthsPassed);
                    CurrentTime = string.Format("{0:0000}.{1:00}.{2:00}", newDate.Year, newDate.Month, newDate.Day);
                }
                else if (_model.GameSpeed == Speed.Normal)
                {
                    // normal mode 1 sec == 1 week
                    int daysPassed = 7;
                    DateTime newDate = startDate.AddDays(daysPassed);
                    CurrentTime = string.Format("{0:0000}.{1:00}.{2:00}", newDate.Year, newDate.Month, newDate.Day);
                }
                else if (_model.GameSpeed == Speed.Slow)
                {
                    // slow mode 1 sec == 1 days
                    int daysPassed =  /*(int)ts.TotalSeconds*/ 1;
                    DateTime newDate = startDate.AddDays(daysPassed);
                    CurrentTime = string.Format("{0:0000}.{1:00}.{2:00}", newDate.Year, newDate.Month, newDate.Day);
                }
            }
        }
       
        private void RefreshTable()
        {
            Fields.Clear();
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    Fields.Add(new GameField
                    {
                        WhichBuilding = _model[i, j].Name,
                        X = i,
                        Y = j,
                        Number = i * Width + j,
                        StepGame = new DelegateCommand(param =>
                        {
                            if(param is GameField field)
                            {
                                try
                                {
                                    if (_model[i,j] is Empty)
                                    {
                                        _model.GameFieldChanged(BuildingArea, field.X, field.Y);
                                    }
                                    
                                }
                                catch { }
                            }
                        })
                    });
                }
            }
        }

        #endregion

        #region Model event handlers
        private void Model_StartNewGame(object? sender, EventArgs e)
        {
            RefreshTable();
        }
        private void Model_FieldChanged(object? sender, FieldChangedEventArgs e)
        {
            Fields[e.X * Width + e.Y].X = e.X; 
            Fields[e.X * Width + e.Y].Y = e.Y;
            Fields[e.X * Width + e.Y].WhichBuilding = e.BArea.Name;

        }

        #endregion

        #region Event Metods
        private void OnGameExit()
        {
            GameExit?.Invoke(this, EventArgs.Empty);
        }
        private void OnSpeedChange(object? param)
        {
            if(SpeedChange is not null)
            {
                SpeedChange(this, new ChangeEventArgs(param.ToString()));
            }
        }
        private void OnBuildingChoice(object? param)
        {
            if(BuildingChoice is not null)
            {
                BuildingChoice(this, new ChangeEventArgs(param.ToString()));
            }
        }

        #endregion



    }
}

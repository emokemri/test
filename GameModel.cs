using AssemblyGameModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyGameModel
{
    /// <summary>
    /// AssemblyGame modellje.
    /// </summary>
    public class GameModel
    {
        #region Fields

        private Int32 height = 20;
        private Int32 width = 31;
        private IArea[,] _gameArea;  // itt tárolunk minden mezőt
        private Speed _gameSpeed;
        private int _tax;
        private int _numberOfZones;
        private int _numberOfPolices;
        private int _numberOfStadiums;
        private int _numberOfSchools;
        private int _numberOfHighSchools;
        private int _numberOfUnis;
        private int _numberOfRoads;
        private int _forests;
        private int _money;
        private int _population;
        //private double _catastrophyProbability;
        private bool _removingMode;
        private bool _zoneMode;
        private bool _metropolisMode;
        private uint _counter;
        private int _currentSumTax;
        private Position _startingRoadPos;
        private Road _startingRoad;
        private HashSet<Residental> _availableResidents;
        private HashSet<Service> _availableServices;
        private HashSet<Industrial> _availableIndustrials;
        private HashSet<Police> _availablePolices;
        private HashSet<Stadium> _availableStadiums;
        private HashSet<Education> _availableEducations;
        private IDataAccess? _dataAccess; // adatelérés
        public String Name = "test";
        public int Satisfaction;

        #endregion

        #region Properties
        public int numberOfZones
        {
            get
            {
                return _numberOfZones;
            }
            set
            {
                _numberOfZones = value;
            }
        }
        public int numberOfPolices
        {
            get
            {
                return _numberOfPolices;
            }
            set
            {
                _numberOfPolices = value;
            }
        }
        public int numberOfStadiums
        {
            get
            {
                return _numberOfStadiums;
            }
            set
            {
                _numberOfStadiums = value;
            }
        }
        public int numberOfSchools
        {
            get
            {
                return _numberOfSchools;
            }
            set
            {
                _numberOfSchools = value;
            }
        }

        public int numberOfHighSchools
        {
            get
            {
                return _numberOfHighSchools;
            }
            set
            {
                _numberOfHighSchools = value;
            }
        }

        public int numberOfUnis
        {
            get
            {
                return _numberOfUnis;
            }
            set
            {
                _numberOfUnis = value;
            }
        }
        public int numberOfRoads
        {
            get
            {
                return _numberOfRoads;
            }
            set
            {
                _numberOfRoads = value;
            }
        }
        public int NumOfForests
        {
            get
            {
                return _forests;
            }
            set
            {
                _forests = value;
            }
        }

        public int CurrentSumTax
        {
            get
            {
                return _currentSumTax;
            }
            set
            {
                _currentSumTax = value;
            }
        }

        public int Population
        {
            get => _population;
            set => _population = value;
        }
        public int ResidentNum => _availableResidents.Count;
        public Boolean RemovingMode
        {
            get => _removingMode;
            set
            {
                if(!MetropolisMode)
                    _removingMode = value;
                if (!ZoneMode)
                    _removingMode = value;
            }
        }

        public Boolean ZoneMode
        {
            get => _zoneMode;
            set
            {
                if (!MetropolisMode)
                    _zoneMode = value;
                if (!RemovingMode)
                    _zoneMode = value;
            }
        }
        public bool MetropolisMode
        {
            get => _metropolisMode;
            set
            {
                if (!RemovingMode)
                    _metropolisMode = value;
                if (!ZoneMode)
                    _metropolisMode = value;
            }
        }
        public int Tax
        {
            get
            {
                return _tax;
            }
            set
            {
                _tax = value;
            }
        }

        public string Time = "";
        public IArea this[int x, int y]
        {
            get
            {
                if (x < 0 || x > _gameArea.GetLength(0))
                    throw new ArgumentOutOfRangeException("Bad column index", nameof(x));
                if (y < 0 || y > _gameArea.GetLength(1))
                    throw new ArgumentOutOfRangeException("Bad row index", nameof(y));
                return _gameArea[x, y];
            }
            set 
            {
                if (x < 0 || x > _gameArea.GetLength(0))
                    throw new ArgumentOutOfRangeException("Bad column index", nameof(x));
                if (y < 0 || y > _gameArea.GetLength(1))
                    throw new ArgumentOutOfRangeException("Bad row index", nameof(y));
                _gameArea[x, y] = value;
            }
        }
        public Speed GameSpeed
        {
            get => _gameSpeed;
            set => _gameSpeed = value;
        }
        public int HEIGHT => height;
        public int WIDTH => width;

        public int Money { get => _money; set => _money = value; }

        #endregion

        #region Events

        public event EventHandler? GameStarted;
        public event EventHandler<FieldChangedEventArgs>? FieldChanged;

        #endregion

        #region Constructor

        /// <summary>
        /// Játék példányosítása.
        /// </summary>
        /// <param name="dataAccess">Az adatelérés.</param>
        public GameModel(IDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
            _gameArea = new IArea[height, width];
            _startingRoadPos = new Position(0, 0);
            _availableResidents = new HashSet<Residental>();
            _availableIndustrials = new HashSet<Industrial>();
            _availableServices = new HashSet<Service>();
            _availablePolices = new HashSet<Police>();
            _availableStadiums = new HashSet<Stadium>();
            _availableEducations = new HashSet<Education>();
            _startingRoad = new Road(_startingRoadPos);
        }

        public GameModel()
        {
            _gameArea = new IArea[height, width];
            _startingRoadPos = new Position(0, 0);
            _availableResidents = new HashSet<Residental>();
            _availableIndustrials = new HashSet<Industrial>();
            _availableServices = new HashSet<Service>();
            _availablePolices = new HashSet<Police>();
            _availableStadiums = new HashSet<Stadium>();
            _availableEducations = new HashSet<Education>();
            _startingRoad = new Road(_startingRoadPos);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Beállítunk minden értéket itt, hogy ne a konstrukorban kelljen.
        /// </summary>
        public void startNewGame()
        {
            _counter = 0;
            _removingMode = false;
            _zoneMode= false;
            _metropolisMode = false;
            //_gameArea = new IArea[height, width];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    _gameArea[i, j] = new Empty(new Position(i, j));
                    _gameArea[i, j].Id = _counter++;
                }
            }
            numberOfPolices = 0;
            numberOfRoads = 1;
            numberOfSchools = 0;
            numberOfHighSchools= 0;
            numberOfUnis = 0;
            numberOfStadiums = 0;
            numberOfZones = 0;
            CurrentSumTax = 0;
            Random rnd = new Random();
            _forests = 10;
            for (int i = 0; i < _forests; i++)
            {
                int x = rnd.Next(height);
                int y = rnd.Next(width);
                if(_gameArea[x, y] is Empty)
                {
                    _gameArea[x, y] = new Forest(new Position(x, y));
                    _gameArea[x, y].Id = _counter++;
                }
                else
                {
                    i--;
                }
            }
            _gameArea[_startingRoadPos.X, _startingRoadPos.Y] = _startingRoad;
            _startingRoad.Id = _counter++;
            _money = 5000;
            _population = 0;
            //_catastrophyProbability = 0;
            _gameSpeed = Speed.Normal;
            _tax = 0;
            OnGameStarted();
        }

        /// <summary>
        /// A Moving függvény a város lakóinak ki- és beköltözéséért felel. 
        /// Figyelembe veszi, hogy jelenleg mennyien laknak az egyes zónákban,
        /// valamint, ha van már legalább 6 lakó zóna, akkor az elégedettség függvényében
        /// költöznek ki és be az emberek
        /// </summary>
        public void Moving()
        {
            Random rnd = new Random();
            if (_availableResidents.Count >= 3 && _availableResidents.Count <= 6)  // játék eleje beköltözés
            {
                foreach (Residental r in _availableResidents)
                {
                    if (r.Fullness < r.Capacity())
                    {
                        int jobless = rnd.Next(5);
                        bool movedin = r.MoveIn(jobless);
                        if (movedin)
                        {
                            _population += jobless;
                            jobless += r.Jobless;
                            if (r.industryWorkersNum < r.serviceWorkersNum)
                            {
                                jobless = checkOpenIndustrialJobs(r, jobless);
                                if (jobless == 0)
                                    continue;
                                jobless = checkOpenServiceJobs(r, jobless);
                            }
                            else
                            {
                                jobless = checkOpenServiceJobs(r, jobless);
                                if (jobless == 0)
                                    continue;
                                jobless = checkOpenIndustrialJobs(r, jobless);
                            }
                            r.Jobless = jobless;
                        }
                    }
                }
            }
            else if (_availableResidents.Count > 6)    // be- vagy kiköltözés az elégedettségtől függően
            {
                foreach (Residental r in _availableResidents)
                {
                    if (r.Satisfaction > 30 && r.ClosestIndustrialDistance >= 5)
                    {
                        if(r.Capacity() > 0)
                        {
                            int jobless = rnd.Next(r.Capacity());
                            bool movedin = r.MoveIn(jobless);
                            if (movedin)
                            {
                                _population += jobless;
                                jobless += r.Jobless;
                                if (r.industryWorkersNum < r.serviceWorkersNum)
                                {
                                    jobless = checkOpenIndustrialJobs(r, jobless);
                                    if (jobless == 0)
                                        continue;
                                    jobless = checkOpenServiceJobs(r, jobless);
                                }
                                else
                                {
                                    jobless = checkOpenServiceJobs(r, jobless);
                                    if (jobless == 0)
                                        continue;
                                    jobless = checkOpenIndustrialJobs(r, jobless);
                                }
                                r.Jobless = jobless;
                            }
                        }
                    }
                    else
                    {
                        int moveout = Math.Min(8, r.Fullness);
                        int didnotleave;
                        if (r.industryWorkersNum < r.serviceWorkersNum)
                        {
                            didnotleave = removeFromService(moveout, r);
                            didnotleave = removeFromIndustry(didnotleave, r);
                        }
                        else
                        {
                            didnotleave = removeFromIndustry(moveout, r);
                            didnotleave = removeFromService(didnotleave, r);
                        }
                        r.Fullness -= (moveout - didnotleave);
                        _population -= (moveout - didnotleave);
                    }
                }
            }
        }

        /// <summary>
        /// Ez a függvény irányítja a _gameArea változtatásait.
        /// </summary>
        /// <param name="bArea"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="Exception"></exception>
        public void GameFieldChanged(IArea bArea, int x, int y)
        {
            if (_removingMode)
            {
                RemoveArea(bArea, x, y);
            }
            else if (_metropolisMode)
            {
                UpgradeMetropolis(bArea, x, y);
            }
            else
            {
                PutDownNewArea(bArea, x, y);
            }
        }

        /// <summary>
        /// Elégedettséget számol, figyelembe véve minden szempontot.
        /// </summary>
        /// <returns></returns>
        public Int32 getSatisfaction()
        {
            if (_population == 0)
                return 0;
            else
            {
                int sum = 0;
                foreach (Residental r in _availableResidents)
                {
                    int x = r.Position.X;
                    int y = r.Position.Y;
                    int forestSatisf = checkForests(r.Position);
                    double industryDist = 0;
                    int industryCount = 0;
                    for (int i = x - 5; i < x + 5; i++)
                    {
                        for (int j = y - 5; j < y + 5; j++)
                        {
                            if (i > 0 && i < height - 1 && j > 0 && j < width - 1)
                            {
                                if (distance(r.Position, _gameArea[i, j].Position) <= 5)
                                {
                                    if (_gameArea[i, j] is Industrial industrial && industrial.Fullness > 0)
                                    {
                                        industryDist += distance(_gameArea[i, j].Position, r.Position);
                                        industryCount++;
                                    }
                                }
                            }
                        }
                    }
                    double policeDist = 0;
                    if (_availablePolices.Count > 0)
                    {
                        foreach (var police in _availablePolices)
                        {
                            policeDist += distance(police.Position, r.Position);
                        }
                        policeDist /= _availablePolices.Count;
                    }
                    double stadiumDist = 0;
                    if (_availableStadiums.Count > 0)
                    {
                        foreach (var stad in _availableStadiums)
                        {
                            double min = distance(stad.Position, r.Position);
                            if (min > distance(new Position(stad.Position.X + 1, stad.Position.Y), r.Position))
                                min = distance(new Position(stad.Position.X + 1, stad.Position.Y), r.Position);
                            if (min > distance(new Position(stad.Position.X, stad.Position.Y + 1), r.Position))
                                min = distance(new Position(stad.Position.X, stad.Position.Y + 1), r.Position);
                            if (min > distance(new Position(stad.Position.X + 1, stad.Position.Y + 1), r.Position))
                                min = distance(new Position(stad.Position.X + 1, stad.Position.Y + 1), r.Position);
                            stadiumDist += min;
                        }
                        stadiumDist /= _availableStadiums.Count;
                    }
                    if (industryCount > 0)
                    {
                        int industrySatisf = (int)(industryDist / industryCount * 10);
                        r.CountSatisfaction(_tax, industrySatisf, forestSatisf, policeDist, stadiumDist);
                    }
                    else
                    {
                        r.CountSatisfaction(_tax, 0, forestSatisf, policeDist, stadiumDist);
                    }
                    sum += r.Satisfaction;
                }
                sum /= _availableResidents.Count;
                sum -= Math.Abs(_availableIndustrials.Count - _availableServices.Count);
                int serviceSatisf = 0;
                foreach (Service s in _availableServices)
                {
                    int partSum = 0;
                    int forestSatisf = checkForests(s.Position);
                    double policeDist = 0;
                    if (_availablePolices.Count > 0)
                    {
                        foreach (var police in _availablePolices)
                        {
                            policeDist += distance(police.Position, s.Position);
                        }
                        policeDist /= _availablePolices.Count;
                    }
                    double stadiumDist = 0;
                    if (_availableStadiums.Count > 0)
                    {
                        foreach (var stad in _availableStadiums)
                        {
                            double min = distance(stad.Position, s.Position);
                            if (min > distance(new Position(stad.Position.X + 1, stad.Position.Y), s.Position))
                                min = distance(new Position(stad.Position.X + 1, stad.Position.Y), s.Position);
                            if (min > distance(new Position(stad.Position.X, stad.Position.Y + 1), s.Position))
                                min = distance(new Position(stad.Position.X, stad.Position.Y + 1), s.Position);
                            if (min > distance(new Position(stad.Position.X + 1, stad.Position.Y + 1), s.Position))
                                min = distance(new Position(stad.Position.X + 1, stad.Position.Y + 1), s.Position);
                            stadiumDist += min;
                        }
                        stadiumDist /= _availableStadiums.Count;
                    }
                    partSum += forestSatisf;
                    if (policeDist < 5)
                        partSum += (int)(5 - policeDist) * 2;
                    if (stadiumDist < 5)
                        partSum += (int)(5 - stadiumDist) * 2;
                    s.Satisfaction = partSum * 2;
                    serviceSatisf += partSum;
                }
                if (_availableServices.Count > 0)
                    serviceSatisf /= _availableServices.Count;
                sum += serviceSatisf;
                if (_money < 0)
                {
                    sum -= 20;
                }
                return sum < 0 ? 0 : (sum > 100 ? 100 : sum);
            }
        }

        /// <summary>
        /// Begyűjti minden lakótól az adót, a jobb végzettségűektől többet.
        /// </summary>
        public void collectTaxes()
        {
            int currentSumTax = 0;
            foreach (Residental res_ in _availableResidents)
            {
                _money += res_.Fullness * _tax;
                //több adó beszedése tanultaktól
                if(_tax > 0)
                {
                    _money += res_.HighSchoolers;
                    _money += res_.BScCitizens * 2;
                    currentSumTax += res_.HighSchoolers + res_.BScCitizens * 2;
                }
                currentSumTax += res_.Fullness * _tax;
            }
            _currentSumTax = currentSumTax;
        }

        /// <summary>
        /// Gimnáziumi végzettséggel rendelkezők számának növelése
        /// </summary>
        public void increaseHighSchoolers()
        {
            double allCitizens = 0;
            double allHighSchoolers = 0;
            foreach (Residental res in _availableResidents)
            {
                allCitizens += res.Fullness;
                allHighSchoolers += res.HighSchoolers;
            }
            foreach (Residental res in _availableResidents)
            {
                //megnézni, hogy kaphatnak-e diplomát, felső korlát ellenőrzése
                if (res.Citizens >= 3 && (allHighSchoolers / allCitizens) * 100 < 70)
                {
                    res.HighSchoolers += 3;
                    res.Citizens -= 3;
                    break;
                }
            }
        }

        /// <summary>
        /// Egyetemi végzettséggel rendelkezők számának növelése
        /// </summary>
        public void increaseBScCitizens()
        {
            double allCitizens = 0;
            double allBScCitizens = 0;
            foreach (Residental res in _availableResidents)
            {
                allCitizens += res.Fullness;
                allBScCitizens += res.BScCitizens;
            }
            foreach (Residental res in _availableResidents)
            {
                //megnezni, hogy kaphatnak-e diplomot, felso korlat ellenorzese
                if (res.Citizens >= 2 && (allBScCitizens / allCitizens) * 100 < 60)
                {
                    res.BScCitizens += 2;
                    res.Citizens -= 2;
                    break;
                }
            }
        }

        /// <summary>
        /// Végzettség nélküliek számának csökkentése
        /// </summary>
        public void decreaseHighSchoolers()
        {
            foreach (Residental res in _availableResidents)
            {
                if (res.HighSchoolers >= 2)
                {
                    res.HighSchoolers -= 2;
                    res.Citizens += 2;
                    break;
                }
            }
        }

        /// <summary>
        /// Gimnáziumi végzettséggel rendelkezők számának csökkentése
        /// </summary>
        public void decreaseBScCitizens()
        {
            foreach (Residental res in _availableResidents)
            {
                if (res.BScCitizens >= 1)
                {
                    res.BScCitizens--;
                    res.Citizens++;
                    break;
                }
            }
        }

        /// <summary>
        /// Katasztrófa bekövetkezése
        /// </summary>
        public Position Catastrophy()
        {
            Random random = new Random();
            int randomNumber = random.Next(0, 6);
            Position catastrophyPosition = new Position(0, 0);
            switch (randomNumber)
            {
                case 0:
                    if (_availableResidents.Count > 0)
                    {
                        Residental attackedResident = _availableResidents.ElementAt(0);

                        _availableResidents.Remove(attackedResident);
                        catastrophyPosition = attackedResident.Position;
                        _gameArea[catastrophyPosition.X, catastrophyPosition.Y] = new Catastrophy(catastrophyPosition);
                        OnFieldChanged(_gameArea[catastrophyPosition.X, catastrophyPosition.Y], catastrophyPosition.X, catastrophyPosition.Y, Money);
                        Satisfaction -= 5;
                        Population -= attackedResident.Fullness;
                        foreach (KeyValuePair<Industrial, int> ind in attackedResident.industryWorkers)
                        {
                            if(ind.Key.Fullness > 0) ind.Key.Fullness -= ind.Value;
                        }
                        foreach (KeyValuePair<Service, int> ind in attackedResident.serviceWorkers)
                        {
                            if (ind.Key.Fullness > 0) ind.Key.Fullness -= ind.Value;
                        }
                        numberOfZones--;
                        return catastrophyPosition;
                    }
                    break;
                case 1:
                    if (_availableEducations.Count > 0)
                    {
                        Education attackedEducation = _availableEducations.ElementAt(0);

                        _availableEducations.Remove(attackedEducation);
                        catastrophyPosition = attackedEducation.Position;
                        if (attackedEducation.Level == EducationLevel.HighSchool)
                        {
                            _gameArea[catastrophyPosition.X, catastrophyPosition.Y] = new Catastrophy(catastrophyPosition);
                            OnFieldChanged(_gameArea[catastrophyPosition.X, catastrophyPosition.Y], catastrophyPosition.X, catastrophyPosition.Y, Money);
                            _gameArea[catastrophyPosition.X, catastrophyPosition.Y + 1] = new Catastrophy(new Position(catastrophyPosition.X, catastrophyPosition.Y + 1));
                            OnFieldChanged(_gameArea[catastrophyPosition.X, catastrophyPosition.Y + 1], catastrophyPosition.X, catastrophyPosition.Y + 1, Money);
                        }
                        else
                        {
                            _gameArea[catastrophyPosition.X, catastrophyPosition.Y] = new Catastrophy(catastrophyPosition);
                            OnFieldChanged(_gameArea[catastrophyPosition.X, catastrophyPosition.Y], catastrophyPosition.X, catastrophyPosition.Y, Money);
                            _gameArea[catastrophyPosition.X, catastrophyPosition.Y + 1] = new Catastrophy(new Position(catastrophyPosition.X, catastrophyPosition.Y + 1));
                            OnFieldChanged(_gameArea[catastrophyPosition.X, catastrophyPosition.Y + 1], catastrophyPosition.X, catastrophyPosition.Y + 1, Money);
                            _gameArea[catastrophyPosition.X + 1, catastrophyPosition.Y] = new Catastrophy(new Position(catastrophyPosition.X + 1, catastrophyPosition.Y));
                            OnFieldChanged(_gameArea[catastrophyPosition.X + 1, catastrophyPosition.Y], catastrophyPosition.X + 1, catastrophyPosition.Y, Money);
                            _gameArea[catastrophyPosition.X + 1, catastrophyPosition.Y + 1] = new Catastrophy(new Position(catastrophyPosition.X + 1, catastrophyPosition.Y + 1));
                            OnFieldChanged(_gameArea[catastrophyPosition.X + 1, catastrophyPosition.Y + 1], catastrophyPosition.X + 1, catastrophyPosition.Y + 1, Money);

                        }
                        numberOfSchools--;
                        Satisfaction -= 5;
                        return catastrophyPosition;
                    }
                    break;
                case 2:
                    if (_availableIndustrials.Count > 0)
                    {
                        Industrial attackedIndustrial = _availableIndustrials.ElementAt(0);

                        _availableIndustrials.Remove(attackedIndustrial);
                        catastrophyPosition = attackedIndustrial.Position;
                        _gameArea[catastrophyPosition.X, catastrophyPosition.Y] = new Catastrophy(catastrophyPosition);
                        OnFieldChanged(_gameArea[catastrophyPosition.X, catastrophyPosition.Y], catastrophyPosition.X, catastrophyPosition.Y, Money);
                        Satisfaction -= 5;
                        numberOfZones--;
                        //új munkahely kereseshez
                        foreach (Residental res in _availableResidents) 
                        {
                            foreach (KeyValuePair<Industrial, int> ind in res.industryWorkers)
                            {
                                if(ind.Key == attackedIndustrial)
                                {
                                    res.Jobless += ind.Value;
                                    break;
                                }
                            }
                        }
                     
                        return catastrophyPosition;
                    }
                    break;
                case 3:
                    if (_availablePolices.Count > 0)
                    {
                        Police attackedPolice = _availablePolices.ElementAt(0);

                        _availablePolices.Remove(attackedPolice);
                        catastrophyPosition = attackedPolice.Position;
                        _gameArea[catastrophyPosition.X, catastrophyPosition.Y] = new Catastrophy(catastrophyPosition);
                        OnFieldChanged(_gameArea[catastrophyPosition.X, catastrophyPosition.Y], catastrophyPosition.X, catastrophyPosition.Y, Money);
                        Satisfaction -= 5;
                        numberOfPolices--;
                        return catastrophyPosition;
                    }
                    break;
                case 4:
                    if (_availableServices.Count > 0)
                    {
                        Service attackedService = _availableServices.ElementAt(0);

                        _availableServices.Remove(attackedService);
                        catastrophyPosition = attackedService.Position;
                        _gameArea[catastrophyPosition.X, catastrophyPosition.Y] = new Catastrophy(catastrophyPosition);
                        OnFieldChanged(_gameArea[catastrophyPosition.X, catastrophyPosition.Y], catastrophyPosition.X, catastrophyPosition.Y, Money);
                        Satisfaction -= 5;
                        numberOfZones--;
                        //új munkahely kereséshez
                        foreach (Residental res in _availableResidents)
                        {
                            foreach (KeyValuePair<Service, int> ser in res.serviceWorkers)
                            {
                                if (ser.Key == attackedService)
                                {
                                    res.Jobless += ser.Value;
                                    break;
                                }
                            }
                        }
                        return catastrophyPosition;
                    }
                    break;
                case 5:
                    if (_availableStadiums.Count > 0)
                    {
                        Stadium attackedStadium = _availableStadiums.ElementAt(0);

                        _availableStadiums.Remove(attackedStadium);
                        catastrophyPosition = attackedStadium.Position;
                        _gameArea[catastrophyPosition.X, catastrophyPosition.Y] = new Catastrophy(catastrophyPosition);
                        OnFieldChanged(_gameArea[catastrophyPosition.X, catastrophyPosition.Y], catastrophyPosition.X, catastrophyPosition.Y, Money);
                        _gameArea[catastrophyPosition.X, catastrophyPosition.Y + 1] = new Catastrophy(new Position(catastrophyPosition.X, catastrophyPosition.Y + 1));
                        OnFieldChanged(_gameArea[catastrophyPosition.X, catastrophyPosition.Y + 1], catastrophyPosition.X, catastrophyPosition.Y + 1, Money);
                        _gameArea[catastrophyPosition.X + 1, catastrophyPosition.Y] = new Catastrophy(new Position(catastrophyPosition.X + 1, catastrophyPosition.Y));
                        OnFieldChanged(_gameArea[catastrophyPosition.X + 1, catastrophyPosition.Y], catastrophyPosition.X + 1, catastrophyPosition.Y, Money);
                        _gameArea[catastrophyPosition.X + 1, catastrophyPosition.Y + 1] = new Catastrophy(new Position(catastrophyPosition.X + 1, catastrophyPosition.Y + 1));
                        OnFieldChanged(_gameArea[catastrophyPosition.X + 1, catastrophyPosition.Y + 1], catastrophyPosition.X + 1, catastrophyPosition.Y + 1, Money);
                        Satisfaction -= 5;
                        numberOfStadiums--;
                        return catastrophyPosition;
                    }
                    break;
            }
            return catastrophyPosition;
        }

        /// <summary>
        /// Katasztrófa után üressé válnak az érintett mezők
        /// </summary>
        /// <param name="catastrophyPosition">A katasztrófa pozíciója</param>
        public void EraseCatastrophy(Position catastrophyPosition)
        {
            if (catastrophyPosition.X == 0 && catastrophyPosition.Y == 0) return;
            _gameArea[catastrophyPosition.X, catastrophyPosition.Y] = new Empty(catastrophyPosition);
            OnFieldChanged(_gameArea[catastrophyPosition.X, catastrophyPosition.Y], catastrophyPosition.X, catastrophyPosition.Y, Money);
            if (_gameArea[catastrophyPosition.X, catastrophyPosition.Y + 1] is Catastrophy)
            {
                _gameArea[catastrophyPosition.X, catastrophyPosition.Y + 1] = new Empty(new Position(catastrophyPosition.X, catastrophyPosition.Y + 1));
                OnFieldChanged(_gameArea[catastrophyPosition.X, catastrophyPosition.Y + 1], catastrophyPosition.X, catastrophyPosition.Y + 1, Money);
            }
            if (_gameArea[catastrophyPosition.X + 1, catastrophyPosition.Y] is Catastrophy)
            {
                _gameArea[catastrophyPosition.X + 1, catastrophyPosition.Y] = new Empty(new Position(catastrophyPosition.X + 1, catastrophyPosition.Y));
                OnFieldChanged(_gameArea[catastrophyPosition.X + 1, catastrophyPosition.Y], catastrophyPosition.X + 1, catastrophyPosition.Y, Money);
            }
            if (_gameArea[catastrophyPosition.X + 1, catastrophyPosition.Y + 1] is Catastrophy)
            {
                _gameArea[catastrophyPosition.X + 1, catastrophyPosition.Y + 1] = new Empty(new Position(catastrophyPosition.X + 1, catastrophyPosition.Y + 1));
                OnFieldChanged(_gameArea[catastrophyPosition.X + 1, catastrophyPosition.Y + 1], catastrophyPosition.X + 1, catastrophyPosition.Y + 1, Money);
            }
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Megpróbál elhelyezni mindenkit ipari munkahelyekre
        /// </summary>
        /// <param name="r"></param>
        /// <param name="people"></param>
        /// <returns>Hány embernek nem sikerült munkát találnia</returns>
        private int checkOpenIndustrialJobs(Residental r, int people)
        {
            foreach (KeyValuePair<double, List<Industrial>> pair in r.Industrials)
            {
                foreach (Industrial i in pair.Value.ToList())
                {
                    if (i.Capacity() >= people) // van mindenkinek hely
                    {
                        i.Fullness += people;
                        if (r.industryWorkers.ContainsKey(i))
                        {
                            r.industryWorkers[i] += people;
                        }
                        else
                        {
                            r.industryWorkers.Add(i, people);
                        }
                        r.industryWorkersNum += people;
                        people = 0;
                    }
                    else // a lehető legtöbbet lehelyezni
                    {
                        int tmp = i.Capacity();
                        i.Fullness += i.Capacity();
                        if (r.industryWorkers.ContainsKey(i))
                        {
                            r.industryWorkers[i] += tmp;
                        }
                        else
                        {
                            r.industryWorkers.Add(i, tmp);
                        }
                        r.industryWorkersNum += tmp;
                        people -= tmp;
                    }
                    if (people == 0)
                    {
                        return 0;
                    }
                }
            }
            return people;
        }

        /// <summary>
        /// Megpróbál elhelyezni mindenkit szolgáltatási munkahelyekre
        /// </summary>
        /// <param name="r"></param>
        /// <param name="people"></param>
        /// <returns>Hány embernek nem sikerült munkát találni</returns>
        private int checkOpenServiceJobs(Residental r, int people)
        {
            foreach (KeyValuePair<double, List<Service>> pair in r.Services)
            {
                foreach (Service s in pair.Value.ToList())
                {
                    if (s.Capacity() >= people) // van mindenkinek hely
                    {
                        s.Fullness += people;
                        if (r.serviceWorkers.ContainsKey(s))
                        {
                            r.serviceWorkers[s] += people;
                        }
                        else
                        {
                            r.serviceWorkers.Add(s, people);
                        }
                        r.serviceWorkersNum += people;
                        people = 0;
                    }
                    else // a lehető legtöbbet lehelyezni
                    {
                        int tmp = s.Capacity();
                        s.Fullness += s.Capacity();
                        if (r.serviceWorkers.ContainsKey(s))
                        {
                            r.serviceWorkers[s] += tmp;
                        }
                        else
                        {
                            r.serviceWorkers.Add(s, tmp);
                        }
                        r.serviceWorkersNum += tmp;
                        people -= tmp;
                    }
                    if (people == 0)
                    {
                        return 0;
                    }
                }
            }
            return people;
        }

        /// <summary>
        /// Akkor hívjuk meg amikor új emberek költöznek be
        /// </summary>
        private void LookForJobs()
        {
            foreach (Residental r in _availableResidents) 
            {
                int jobless = r.Jobless;
                if (r.industryWorkersNum < r.serviceWorkersNum)
                {
                    jobless = checkOpenIndustrialJobs(r, jobless);
                    if (jobless == 0)
                        continue;
                    jobless = checkOpenServiceJobs(r, jobless);
                }
                else
                {
                    jobless = checkOpenServiceJobs(r, jobless);
                    if (jobless == 0)
                        continue;
                    jobless = checkOpenIndustrialJobs(r, jobless);
                }
                r.Jobless = jobless;
            }
        }

        /// <summary>
        /// Kiköltözéskor, kezeljük, hogy az emberek már nem dolgoznak ott
        /// </summary>
        /// <param name="moveout"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        private int removeFromIndustry(int moveout, Residental r)
        {
            foreach (var pair in r.industryWorkers)
            {
                if (pair.Value >= moveout)
                {
                    pair.Key.Fullness -= moveout;
                    r.industryWorkers[pair.Key] -= moveout;
                    r.industryWorkersNum -= moveout;
                    moveout = 0;
                    return moveout;
                }
                else
                {
                    int tmp = pair.Value;
                    moveout -= pair.Value;
                    pair.Key.Fullness -= tmp;
                    r.industryWorkers[pair.Key] = 0;
                    r.industryWorkersNum -= tmp;
                }
            }
            return moveout;
        }

        /// <summary>
        /// Kiköltözéskor, kezeljük, hogy az emberek már nem dolgoznak ott
        /// </summary>
        /// <param name="moveout"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        private int removeFromService(int moveout, Residental r)
        {
            foreach (var pair in r.serviceWorkers)
            {
                if (pair.Value >= moveout)
                {
                    pair.Key.Fullness -= moveout;
                    r.serviceWorkers[pair.Key] -= moveout;
                    r.serviceWorkersNum -= moveout;
                    moveout = 0;
                    return moveout;
                }
                else
                {
                    int tmp = pair.Value;
                    moveout -= pair.Value;
                    pair.Key.Fullness -= tmp;
                    r.serviceWorkers[pair.Key] = 0;
                    r.serviceWorkersNum -= tmp;
                }
            }
            return moveout;
        }

        /// <summary>
        /// Az elégedettséghez számoljuk a közelben lévő erdők számát.
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        private int checkForests(Position r)
        {
            int x = r.X; int y = r.Y;
            int forestnum = 0;
            for (int i = 0; i < 3; i++)
            {
                if (x - i > 0 && _gameArea[x - i, y] is Forest)
                {
                    if (((Forest)_gameArea[x - i, y]).ImageId <= 3)     // az imageId az valójában az életkora az erdőnek
                        forestnum += 1;
                    else if (((Forest)_gameArea[x - i, y]).ImageId <= 7)
                        forestnum += 2;
                    else if (((Forest)_gameArea[x - i, y]).ImageId > 7)
                        forestnum += 3;
                }
                if (x + i < height - 1 && _gameArea[x + i, y] is Forest)
                {
                    if (((Forest)_gameArea[x + i, y]).ImageId <= 3)
                        forestnum += 1;
                    else if (((Forest)_gameArea[x + i, y]).ImageId <= 7)
                        forestnum += 2;
                    else if (((Forest)_gameArea[x + i, y]).ImageId > 7)
                        forestnum += 3;
                }
                if (y - i > 0 && _gameArea[x, y - i] is Forest)
                {
                    if (((Forest)_gameArea[x, y - i]).ImageId <= 3)
                        forestnum += 1;
                    else if (((Forest)_gameArea[x, y - i]).ImageId <= 7)
                        forestnum += 2;
                    else if (((Forest)_gameArea[x, y - i]).ImageId > 7)
                        forestnum += 3;
                }
                if (y + i < width - 1 && _gameArea[x, y + i] is Forest)
                {
                    if (((Forest)_gameArea[x, y + i]).ImageId <= 3)
                        forestnum += 1;
                    else if (((Forest)_gameArea[x, y + i]).ImageId <= 7)
                        forestnum += 2;
                    else if (((Forest)_gameArea[x, y + i]).ImageId > 7)
                        forestnum += 3;
                }
            }
            if (x - 1 > 0)
            {
                if (y - 1 > 0 && _gameArea[x - 1, y - 1] is Forest)
                {
                    if (((Forest)_gameArea[x - 1, y - 1]).ImageId <= 3)
                        forestnum += 1;
                    else if (((Forest)_gameArea[x - 1, y - 1]).ImageId <= 7)
                        forestnum += 2;
                    else if (((Forest)_gameArea[x - 1, y - 1]).ImageId > 7)
                        forestnum += 3;
                }
                if (y + 1 < width - 1 && _gameArea[x - 1, y + 1] is Forest)
                {
                    if (((Forest)_gameArea[x - 1, y + 1]).ImageId <= 3)
                        forestnum += 1;
                    else if (((Forest)_gameArea[x - 1, y + 1]).ImageId <= 7)
                        forestnum += 2;
                    else if (((Forest)_gameArea[x - 1, y + 1]).ImageId > 7)
                        forestnum += 3;
                }
            }
            if (x + 1 < height - 1)
            {
                if (y - 1 > 0 && _gameArea[x + 1, y - 1] is Forest)
                {
                    if (((Forest)_gameArea[x + 1, y - 1]).ImageId <= 3)
                        forestnum += 1;
                    else if (((Forest)_gameArea[x + 1, y - 1]).ImageId <= 7)
                        forestnum += 2;
                    else if (((Forest)_gameArea[x + 1, y - 1]).ImageId > 7)
                        forestnum += 3;
                }
                if (y + 1 < width - 1 && _gameArea[x + 1, y + 1] is Forest)
                {
                    if (((Forest)_gameArea[x + 1, y + 1]).ImageId <= 3)
                        forestnum += 1;
                    else if (((Forest)_gameArea[x + 1, y + 1]).ImageId <= 7)
                        forestnum += 2;
                    else if (((Forest)_gameArea[x + 1, y + 1]).ImageId > 7)
                        forestnum += 3;
                }
            }
            return forestnum;
        }

        /// <summary>
        /// Egyszerű euklideszi norma számítás
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private double distance(Position a, Position b)
        {
            return Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
        }

        /// <summary>
        /// Zónák eltávolítása
        /// </summary>
        /// <param name="bArea"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <exception cref="ArgumentException"></exception>
        private void RemoveArea(IArea bArea, int x, int y)
        {
            uint id = _gameArea[x, y].Id;
            int h = _gameArea[x, y].Height;
            int w = _gameArea[x, y].Width;
            if (x == _startingRoadPos.X && y == _startingRoadPos.Y)
            {
                throw new ArgumentException("Az M1-es bevezőt nem lehet eltávolítani!");
            }
            else if (_gameArea[x, y] is Residental && ((Residental)_gameArea[x, y]).Fullness > 0)
            {
                throw new ArgumentException("Emberek élnek ebben a zónában!" + Environment.NewLine + "Nem lehet eltávolítani!");
            }
            else if ((_gameArea[x, y] is Industrial && ((Industrial)_gameArea[x, y]).Fullness > 0) ||
                (_gameArea[x, y] is Service && ((Service)_gameArea[x, y]).Fullness > 0))
            {
                throw new ArgumentException("Emberek dolgoznak ebben a zónában!" + Environment.NewLine + "Nem lehet eltávolítani!");
            }
            else if (_gameArea[x, y] is Road)
            {
                BFS((Road)_gameArea[x, y]);
                foreach (Residental r in _availableResidents)
                {
                    checkSurroundings(r.Position, false);
                }
                foreach (Industrial i in _availableIndustrials)
                {
                    checkSurroundings(i.Position, false);
                }
                foreach (Service s in _availableServices)
                {
                    checkSurroundings(s.Position, false);
                }
                foreach (Police p in _availablePolices)
                {
                    checkSurroundings(p.Position, false);
                }
                foreach (Stadium s in _availableStadiums)
                {
                    checkSurroundings(s.Position, false);
                }
                foreach (Education e in _availableEducations)
                {
                    checkSurroundings(e.Position, false);
                }
                numberOfRoads--;
            }
            else if (_gameArea[x, y] is Service)
            {
                foreach (Residental r in _availableResidents)
                {
                    double distFromRes = distance(r.Position, _gameArea[x, y].Position);
                    if (r.Services.ContainsKey(distFromRes))
                    {
                        numberOfZones--;
                        r.Services[distFromRes].Remove((Service)_gameArea[x, y]);
                    }
                }
                _availableServices.Remove((Service)_gameArea[x, y]);
            }
            else if (_gameArea[x, y] is Industrial)
            {
                foreach (Residental r in _availableResidents)
                {
                    double distFromRes = distance(r.Position, _gameArea[x, y].Position);
                    if (r.Industrials.ContainsKey(distFromRes))
                    {
                        numberOfZones--;
                        r.Industrials[distFromRes].Remove((Industrial)_gameArea[x, y]);
                    }
                }
                _availableIndustrials.Remove((Industrial)_gameArea[x, y]);
            }
            else if (_gameArea[x, y] is Residental)
            {
                numberOfZones--;
                _availableResidents.Remove((Residental)_gameArea[x, y]);
            }
            else if (_gameArea[x, y] is Police)
            {
                _availablePolices.Remove((Police)_gameArea[x, y]);
                numberOfPolices--;
            }
            else if (_gameArea[x, y] is Stadium)
            {
                _availableStadiums.Remove((Stadium)_gameArea[x, y]);
                numberOfStadiums--;
            }
            else if (_gameArea[x, y] is Education ed)
            {
                _availableEducations.Remove((Education)_gameArea[x, y]);
                numberOfSchools--;
                if (ed.Level is EducationLevel.HighSchool) numberOfHighSchools--;
                else numberOfUnis--;
            }
            else if (_gameArea[x, y] is Forest)
            {
                NumOfForests--;
            }
            Money += _gameArea[x, y].DeletePrice;
            for (int i = x - h + 1; i < x + h; i++)
            {
                if (i >= 0 && i < height)
                {
                    for (int j = y - w + 1; j < y + w; j++)
                    {
                        if (j >= 0 && _gameArea[i, j].Id == id && j < width)
                        {
                            _gameArea[i, j] = new Empty(new Position(i, j));
                            _gameArea[i, j].Id = ++_counter;
                            OnFieldChanged(_gameArea[i, j], i, j, Money);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Zónák fejlesztése
        /// </summary>
        /// <param name="bArea"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <exception cref="Exception"></exception>
        private void UpgradeMetropolis(IArea bArea, int x, int y)
        {
            if (bArea is Residental || bArea is Service || bArea is Industrial)
            {
                Zone zone = bArea as Zone;
                if (zone.Fullness > 0)
                {
                    if (zone.Metropolis == MetropolisLevel.Level1)
                    {
                        Money -= 100;
                        zone.Metropolis = MetropolisLevel.Level2;
                        zone.ImageId = 4;
                        OnFieldChanged(zone, x, y, Money);
                    }
                    else if (zone.Metropolis == MetropolisLevel.Level2)
                    {
                        Money -= 150;
                        zone.Metropolis = MetropolisLevel.Level3;
                        zone.ImageId = 5;
                        OnFieldChanged(zone, x, y, Money);
                    }
                    else if (zone.Metropolis == MetropolisLevel.Level3)
                    {
                        throw new Exception("Nem lehet tovább fejleszteni!");
                    }
                }
                else
                {
                    throw new Exception("Itt még nem lakik senki, nem lehet fejleszteni!");
                }

            }
            else
            {
                throw new Exception("Nem zóna mező, nem lehet fejleszteni!");
            }
        }

        /// <summary>
        /// Zónák lehelyezése
        /// </summary>
        /// <param name="bArea"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <exception cref="ArgumentException"></exception>
        private void PutDownNewArea(IArea bArea, int x, int y)
        {
            bArea.Position = new Position(x, y);
            // ellenőrizzük, hogy üres-e a terület, ahová leraknánk az újat
            for (int i = 0; i < bArea.Height; i++)
            {
                for (int j = 0; j < bArea.Width; j++)
                {
                    if (x + i >= height || y + j >= width)
                    {
                        throw new ArgumentException("Nincs elég hely");
                    }
                    if (_gameArea[x + i, y + j] is not Empty)
                    {
                        throw new ArgumentException("Nem üres pozíció");
                    }
                }
            }
            // ha üres volt, lerakjuk
            uint id = ++_counter;
            int whichPiece = 1;
            for (int i = 0; i < bArea.Height; i++)
            {
                for (int j = 0; j < bArea.Width; j++)
                {
                    _gameArea[x + i, y + j] = bArea;
                    _gameArea[x + i, y + j].Id = id;
                    _gameArea[x + i, y + j].ImageId = whichPiece;
                    OnFieldChanged(_gameArea[x + i, y + j], x + i, y + j, Money);
                    whichPiece++;
                }
            }
            if (bArea is Service || bArea is Industrial || bArea is Police || bArea is Stadium || bArea is Education)
            {
                checkSurroundings(bArea.Position, true);
                LookForJobs();
            }
            else if (bArea is Residental)
            {
                checkSurroundings(bArea.Position, true);
                LookForJobs();
            }
            else if (bArea is Road)
            {
                BFS(null!);
            }


            if (bArea is Residental || bArea is Service || bArea is Industrial)
                _numberOfZones++;
            else if (bArea is Police)
                _numberOfPolices++;
            else if (bArea is Stadium)
                _numberOfStadiums++;
            else if (bArea is Education ed)
            {
                _numberOfSchools++;
                if (ed.Level == EducationLevel.HighSchool) _numberOfHighSchools++;
                else _numberOfUnis++;
            }
            else if (bArea is Road)
                _numberOfRoads++;
            else if (bArea is Forest)
                _forests++;
            Money -= bArea.Price;

        }

        /// <summary>
        /// Ellenőrzi a zónák környezetét. 
        /// Kell a szélességi bejáráshoz, ellenőrizzük, hogy el lehet-e távolítani egy utat biztonságosan.
        /// Ha új épületet helyezünk le, akkor csekkoljuk, hogy elérhető-e a startból.
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="adding"></param>
        /// <exception cref="ArgumentException"></exception>
        private void checkSurroundings(Position pos, bool adding)
        {
            int dist = Int32.MaxValue;
            Road parent = null!;
            int count = 0;
            if (_gameArea[pos.X, pos.Y] is Stadium || (_gameArea[pos.X, pos.Y] is Education && ((Education)_gameArea[pos.X, pos.Y]).Level == EducationLevel.University) )
            {
                if (pos.X > 0 && _gameArea[pos.X - 1, pos.Y] is Road && ((Road)_gameArea[pos.X - 1, pos.Y]).NumOfVisits == _startingRoad.NumOfVisits)    // NumOfVisits mutatja, hogy elérehtő-e a startból a Road mező
                {
                    count++;
                    if (dist > ((Road)_gameArea[pos.X - 1, pos.Y]).DistFromStart + 1)
                    {
                        dist = ((Road)_gameArea[pos.X - 1, pos.Y]).DistFromStart + 1;
                        parent = (Road)_gameArea[pos.X - 1, pos.Y];
                    }
                }
                if (pos.X > 0 && pos.Y < width - 1 && _gameArea[pos.X - 1, pos.Y + 1] is Road && ((Road)_gameArea[pos.X - 1, pos.Y + 1]).NumOfVisits == _startingRoad.NumOfVisits)    // NumOfVisits mutatja, hogy elérehtő-e a startból a Road mező
                {
                    count++;
                    if (dist > ((Road)_gameArea[pos.X - 1, pos.Y + 1]).DistFromStart + 1)
                    {
                        dist = ((Road)_gameArea[pos.X - 1, pos.Y + 1]).DistFromStart + 1;
                        parent = (Road)_gameArea[pos.X - 1, pos.Y + 1];
                    }
                }
                if (pos.Y > 0 && _gameArea[pos.X, pos.Y - 1] is Road && ((Road)_gameArea[pos.X, pos.Y - 1]).NumOfVisits == _startingRoad.NumOfVisits)
                {
                    count++;
                    if (dist > ((Road)_gameArea[pos.X, pos.Y - 1]).DistFromStart + 1)
                    {
                        dist = ((Road)_gameArea[pos.X, pos.Y - 1]).DistFromStart + 1;
                        parent = (Road)_gameArea[pos.X, pos.Y - 1];
                    }
                }
                if (pos.Y > 0 && pos.X < height - 1 && _gameArea[pos.X + 1, pos.Y - 1] is Road && ((Road)_gameArea[pos.X + 1, pos.Y - 1]).NumOfVisits == _startingRoad.NumOfVisits)
                {
                    count++;
                    if (dist > ((Road)_gameArea[pos.X + 1, pos.Y - 1]).DistFromStart + 1)
                    {
                        dist = ((Road)_gameArea[pos.X + 1, pos.Y - 1]).DistFromStart + 1;
                        parent = (Road)_gameArea[pos.X + 1, pos.Y - 1];
                    }
                }
                if (pos.X < height - 2 && _gameArea[pos.X + 2, pos.Y] is Road && ((Road)_gameArea[pos.X + 2, pos.Y]).NumOfVisits == _startingRoad.NumOfVisits)
                {
                    count++;
                    if (dist > ((Road)_gameArea[pos.X + 2, pos.Y]).DistFromStart + 1)
                    {
                        dist = ((Road)_gameArea[pos.X + 2, pos.Y]).DistFromStart + 1;
                        parent = (Road)_gameArea[pos.X + 2, pos.Y];
                    }
                }
                if (pos.X < height - 2 && pos.Y < width - 1 && _gameArea[pos.X + 2, pos.Y + 1] is Road && ((Road)_gameArea[pos.X + 2, pos.Y + 1]).NumOfVisits == _startingRoad.NumOfVisits)
                {
                    count++;
                    if (dist > ((Road)_gameArea[pos.X + 2, pos.Y + 1]).DistFromStart + 1)
                    {
                        dist = ((Road)_gameArea[pos.X + 2, pos.Y + 1]).DistFromStart + 1;
                        parent = (Road)_gameArea[pos.X + 2, pos.Y + 1];
                    }
                }
                if (pos.Y < width - 2 && _gameArea[pos.X, pos.Y + 2] is Road && ((Road)_gameArea[pos.X, pos.Y + 2]).NumOfVisits == _startingRoad.NumOfVisits)
                {
                    count++;
                    if (dist > ((Road)_gameArea[pos.X, pos.Y + 2]).DistFromStart + 1)
                    {
                        dist = ((Road)_gameArea[pos.X, pos.Y + 2]).DistFromStart + 1;
                        parent = (Road)_gameArea[pos.X, pos.Y + 2];
                    }
                }
                if (pos.Y < width - 2 && pos.X < height - 1 && _gameArea[pos.X + 1, pos.Y + 2] is Road && ((Road)_gameArea[pos.X + 1, pos.Y + 2]).NumOfVisits == _startingRoad.NumOfVisits)
                {
                    count++;
                    if (dist > ((Road)_gameArea[pos.X + 1, pos.Y + 2]).DistFromStart + 1)
                    {
                        dist = ((Road)_gameArea[pos.X + 1, pos.Y + 2]).DistFromStart + 1;
                        parent = (Road)_gameArea[pos.X + 1, pos.Y + 2];
                    }
                }
            }
            else if (_gameArea[pos.X, pos.Y] is Education && ((Education)_gameArea[pos.X, pos.Y]).Level == EducationLevel.HighSchool)
            {
                if (pos.X > 0 && _gameArea[pos.X - 1, pos.Y] is Road && ((Road)_gameArea[pos.X - 1, pos.Y]).NumOfVisits == _startingRoad.NumOfVisits)    // NumOfVisits mutatja, hogy elérehtő-e a startból a Road mező
                {
                    count++;
                    if (dist > ((Road)_gameArea[pos.X - 1, pos.Y]).DistFromStart + 1)
                    {
                        dist = ((Road)_gameArea[pos.X - 1, pos.Y]).DistFromStart + 1;
                        parent = (Road)_gameArea[pos.X - 1, pos.Y];
                    }
                }
                if (pos.X > 0 && pos.Y < width - 1 && _gameArea[pos.X - 1, pos.Y + 1] is Road && ((Road)_gameArea[pos.X - 1, pos.Y + 1]).NumOfVisits == _startingRoad.NumOfVisits)    // NumOfVisits mutatja, hogy elérehtő-e a startból a Road mező
                {
                    count++;
                    if (dist > ((Road)_gameArea[pos.X - 1, pos.Y + 1]).DistFromStart + 1)
                    {
                        dist = ((Road)_gameArea[pos.X - 1, pos.Y + 1]).DistFromStart + 1;
                        parent = (Road)_gameArea[pos.X - 1, pos.Y + 1];
                    }
                }
                if (pos.Y > 0 && _gameArea[pos.X, pos.Y - 1] is Road && ((Road)_gameArea[pos.X, pos.Y - 1]).NumOfVisits == _startingRoad.NumOfVisits)
                {
                    count++;
                    if (dist > ((Road)_gameArea[pos.X, pos.Y - 1]).DistFromStart + 1)
                    {
                        dist = ((Road)_gameArea[pos.X, pos.Y - 1]).DistFromStart + 1;
                        parent = (Road)_gameArea[pos.X, pos.Y - 1];
                    }
                }
                if (pos.X < height - 1 && _gameArea[pos.X + 1, pos.Y] is Road && ((Road)_gameArea[pos.X + 1, pos.Y]).NumOfVisits == _startingRoad.NumOfVisits)
                {
                    count++;
                    if (dist > ((Road)_gameArea[pos.X + 1, pos.Y]).DistFromStart + 1)
                    {
                        dist = ((Road)_gameArea[pos.X + 1, pos.Y]).DistFromStart + 1;
                        parent = (Road)_gameArea[pos.X + 1, pos.Y];
                    }
                }
                if (pos.X < height - 1 && pos.Y < width - 1 && _gameArea[pos.X + 1, pos.Y + 1] is Road && ((Road)_gameArea[pos.X + 1, pos.Y + 1]).NumOfVisits == _startingRoad.NumOfVisits)
                {
                    count++;
                    if (dist > ((Road)_gameArea[pos.X + 1, pos.Y + 1]).DistFromStart + 1)
                    {
                        dist = ((Road)_gameArea[pos.X + 1, pos.Y + 1]).DistFromStart + 1;
                        parent = (Road)_gameArea[pos.X + 1, pos.Y + 1];
                    }
                }
                if (pos.Y < width - 2 && _gameArea[pos.X, pos.Y + 2] is Road && ((Road)_gameArea[pos.X, pos.Y + 2]).NumOfVisits == _startingRoad.NumOfVisits)
                {
                    count++;
                    if (dist > ((Road)_gameArea[pos.X, pos.Y + 2]).DistFromStart + 1)
                    {
                        dist = ((Road)_gameArea[pos.X, pos.Y + 2]).DistFromStart + 1;
                        parent = (Road)_gameArea[pos.X, pos.Y + 2];
                    }
                }
            }
            else
            {
                if (pos.X > 0 && _gameArea[pos.X - 1, pos.Y] is Road && ((Road)_gameArea[pos.X - 1, pos.Y]).NumOfVisits == _startingRoad.NumOfVisits)    // NumOfVisits mutatja, hogy elérehtő-e a startból a Road mező
                {
                    count++;
                    if (dist > ((Road)_gameArea[pos.X - 1, pos.Y]).DistFromStart + 1)
                    {
                        dist = ((Road)_gameArea[pos.X - 1, pos.Y]).DistFromStart + 1;
                        parent = (Road)_gameArea[pos.X - 1, pos.Y];
                    }
                }
                if (pos.Y > 0 && _gameArea[pos.X, pos.Y - 1] is Road && ((Road)_gameArea[pos.X, pos.Y - 1]).NumOfVisits == _startingRoad.NumOfVisits)
                {
                    count++;
                    if (dist > ((Road)_gameArea[pos.X, pos.Y - 1]).DistFromStart + 1)
                    {
                        dist = ((Road)_gameArea[pos.X, pos.Y - 1]).DistFromStart + 1;
                        parent = (Road)_gameArea[pos.X, pos.Y - 1];
                    }
                }
                if (pos.X < height - 1 && _gameArea[pos.X + 1, pos.Y] is Road && ((Road)_gameArea[pos.X + 1, pos.Y]).NumOfVisits == _startingRoad.NumOfVisits)
                {
                    count++;
                    if (dist > ((Road)_gameArea[pos.X + 1, pos.Y]).DistFromStart + 1)
                    {
                        dist = ((Road)_gameArea[pos.X + 1, pos.Y]).DistFromStart + 1;
                        parent = (Road)_gameArea[pos.X + 1, pos.Y];
                    }
                }
                if (pos.Y < width - 1 && _gameArea[pos.X, pos.Y + 1] is Road && ((Road)_gameArea[pos.X, pos.Y + 1]).NumOfVisits == _startingRoad.NumOfVisits)
                {
                    count++;
                    if (dist > ((Road)_gameArea[pos.X, pos.Y + 1]).DistFromStart + 1)
                    {
                        dist = ((Road)_gameArea[pos.X, pos.Y + 1]).DistFromStart + 1;
                        parent = (Road)_gameArea[pos.X, pos.Y + 1];
                    }
                }
            }
            if(count == 0 && !adding)
            {
                throw new ArgumentException($"Nem lehet eltávolítani ezt az utat!\nA(z) {_gameArea[pos.X, pos.Y].Nev} mezőt nem lehetne elérni, ha eltávolítanánk ezt az utat!");
            }
            if (_gameArea[pos.X, pos.Y] is Residental)
            {
                if (dist < Int32.MaxValue)
                {
                    _availableResidents.Add((Residental)_gameArea[pos.X, pos.Y]);
                    foreach (Service s in _availableServices)
                    {
                        double distFromRes = distance(s.Position, _gameArea[pos.X, pos.Y].Position);
                        if (((Residental)_gameArea[pos.X, pos.Y]).Services.ContainsKey(distFromRes))
                        {
                            ((Residental)_gameArea[pos.X, pos.Y]).Services[distFromRes].Add(s);
                        }
                        else
                        {
                            ((Residental)_gameArea[pos.X, pos.Y]).Services.Add(distFromRes, new List<Service>() {s});
                        }
                    }
                    foreach (Industrial i in _availableIndustrials)
                    {
                        double distFromRes = distance(i.Position, _gameArea[pos.X, pos.Y].Position);
                        if (((Residental)_gameArea[pos.X, pos.Y]).Industrials.ContainsKey(distFromRes))
                        {
                            ((Residental)_gameArea[pos.X, pos.Y]).Industrials[distFromRes].Add(i);
                        }
                        else
                        {
                            ((Residental)_gameArea[pos.X, pos.Y]).Industrials.Add(distFromRes, new List<Industrial>() { i });
                        }
                    }
                }
                ((Residental)_gameArea[pos.X, pos.Y]).NumOfRoads = count;
                ((Residental)_gameArea[pos.X,pos.Y]).DistFromStart = dist;
                ((Residental)_gameArea[pos.X, pos.Y]).ParentRoad = parent;
            }
            else if (_gameArea[pos.X, pos.Y] is Service)
            {
                if (dist < Int32.MaxValue)
                {
                    _availableServices.Add((Service)_gameArea[pos.X, pos.Y]);
                    foreach (Residental r in _availableResidents)
                    {
                        double distFromRes = distance(r.Position, _gameArea[pos.X, pos.Y].Position);
                        if (r.Services.ContainsKey(distFromRes))
                        {
                            r.Services[distFromRes].Add((Service)_gameArea[pos.X, pos.Y]);
                        }
                        else
                        {
                            r.Services.Add(distFromRes, new List<Service>() { (Service)_gameArea[pos.X, pos.Y] });
                        }
                    }
                }
                ((Service)_gameArea[pos.X, pos.Y]).NumOfRoads = count;
                ((Service)_gameArea[pos.X, pos.Y]).DistFromStart = dist;
                ((Service)_gameArea[pos.X, pos.Y]).ParentRoad = parent;
            }
            else if (_gameArea[pos.X, pos.Y] is Industrial)
            {
                if (dist < Int32.MaxValue)
                {
                    _availableIndustrials.Add((Industrial)_gameArea[pos.X, pos.Y]);
                    foreach (Residental r in _availableResidents)
                    {
                        double distFromRes = distance(r.Position, _gameArea[pos.X, pos.Y].Position);
                        if (r.Industrials.ContainsKey(distFromRes))
                        {
                            (r.Industrials[distFromRes]).Add((Industrial)_gameArea[pos.X, pos.Y]);
                        }
                        else
                        {
                            r.Industrials.Add(distFromRes, new List<Industrial>() { (Industrial)_gameArea[pos.X, pos.Y] });
                        }
                    }
                }
                ((Industrial)_gameArea[pos.X, pos.Y]).NumOfRoads = count;
                ((Industrial)_gameArea[pos.X, pos.Y]).DistFromStart = dist;
                ((Industrial)_gameArea[pos.X, pos.Y]).ParentRoad = parent;
            }
            else if (_gameArea[pos.X, pos.Y] is Police police)
            {
                if (dist < Int32.MaxValue)
                {
                    _availablePolices.Add(police);
                }
                police.NumOfRoads = count;
                police.DistFromStart = dist;
                police.ParentRoad = parent;
            }
            else if (_gameArea[pos.X, pos.Y] is Stadium)
            {
                if (dist < Int32.MaxValue)
                {
                    _availableStadiums.Add((Stadium)_gameArea[pos.X, pos.Y]);
                }
                ((Stadium)_gameArea[pos.X, pos.Y]).NumOfRoads = count;
                ((Stadium)_gameArea[pos.X, pos.Y]).DistFromStart = dist;
                ((Stadium)_gameArea[pos.X, pos.Y]).ParentRoad = parent;
            }
            else if (_gameArea[pos.X, pos.Y] is Education)
            {
                if (dist < Int32.MaxValue)
                {
                    _availableEducations.Add((Education)_gameArea[pos.X, pos.Y]);
                }
                ((Education)_gameArea[pos.X, pos.Y]).NumOfRoads = count;
                ((Education)_gameArea[pos.X, pos.Y]).DistFromStart = dist;
                ((Education)_gameArea[pos.X, pos.Y]).ParentRoad = parent;
            }
        }
     
        /// <summary>
        /// Szélességi gráfbejárás implementálása dr. Ásványi Tibor előadásának algritmusa alapján
        /// </summary>
        /// <param name="ignored"></param>
        private void BFS(Road ignored)
        {
            _startingRoad.ParentRoad = null!;
            _startingRoad.DistFromStart = 0;
            _startingRoad.NumOfVisits++;    // color(s) := grey, az előadáson tanult algoritmusban
            Position ignoredPos;
            if (ignored != null)
                ignoredPos = ignored.Position;
            else
                ignoredPos = new Position(-1, -1);
            Queue<Road> Q = new Queue<Road>();
            Q.Enqueue(_startingRoad);
            while (Q.Count > 0)
            {
                Road selectedRoad = Q.Dequeue();
                Position roadPos = selectedRoad.Position;
                if (roadPos.Y > 0 && _gameArea[roadPos.X, roadPos.Y - 1] is Road && (roadPos.X != ignoredPos.X || roadPos.Y - 1 != ignoredPos.Y)) // balra Road van
                {
                    if(((Road)_gameArea[roadPos.X, roadPos.Y - 1]).NumOfVisits < selectedRoad.NumOfVisits)  // ha még nem vizsgáltuk meg az adott Roadot
                    {
                        ((Road)_gameArea[roadPos.X, roadPos.Y - 1]).DistFromStart = selectedRoad.DistFromStart + 1;
                        ((Road)_gameArea[roadPos.X, roadPos.Y - 1]).ParentRoad = selectedRoad;
                        ((Road)_gameArea[roadPos.X, roadPos.Y - 1]).NumOfVisits = selectedRoad.NumOfVisits;
                        Q.Enqueue((Road)_gameArea[roadPos.X, roadPos.Y - 1]);
                    }
                }
                else if (roadPos.Y > 0 && 
                    (_gameArea[roadPos.X, roadPos.Y - 1] is Residental ||   // balra Residental van
                    _gameArea[roadPos.X, roadPos.Y - 1] is Service ||       // balra Service van
                    _gameArea[roadPos.X, roadPos.Y - 1] is Industrial))     // balra Industrial van
                {
                    checkSurroundings(new Position(roadPos.X, roadPos.Y - 1), ignoredPos.X == -1);
                }
                if (roadPos.X > 0 && _gameArea[roadPos.X - 1, roadPos.Y] is Road && (roadPos.X - 1 != ignoredPos.X || roadPos.Y != ignoredPos.Y)) // felette Road van
                {
                    if(((Road)_gameArea[roadPos.X - 1, roadPos.Y]).NumOfVisits < selectedRoad.NumOfVisits)  // ha még nem vizsgáltuk meg az adott Roadot
                    {
                        ((Road)_gameArea[roadPos.X - 1, roadPos.Y]).DistFromStart = selectedRoad.DistFromStart + 1;
                        ((Road)_gameArea[roadPos.X - 1, roadPos.Y]).ParentRoad = selectedRoad;
                        ((Road)_gameArea[roadPos.X - 1, roadPos.Y]).NumOfVisits = selectedRoad.NumOfVisits;
                        Q.Enqueue(((Road)_gameArea[roadPos.X - 1, roadPos.Y]));
                    }
                }
                else if (roadPos.X > 0 && 
                    (_gameArea[roadPos.X - 1, roadPos.Y] is Residental ||   // felette Residental van
                    _gameArea[roadPos.X - 1, roadPos.Y] is Service ||       // felette Service van
                    _gameArea[roadPos.X - 1, roadPos.Y] is Industrial))     // felette Industrial van 
                {
                    checkSurroundings(new Position(roadPos.X - 1, roadPos.Y), ignoredPos.X == -1);
                }
                if (roadPos.Y < width - 1 && _gameArea[roadPos.X, roadPos.Y + 1] is Road && (roadPos.X != ignoredPos.X || roadPos.Y + 1 != ignoredPos.Y)) // jobbra Road van
                {
                    if(((Road)_gameArea[roadPos.X, roadPos.Y + 1]).NumOfVisits < selectedRoad.NumOfVisits)  // ha még nem vizsgáltuk meg az adott Roadot
                    {
                        ((Road)_gameArea[roadPos.X, roadPos.Y + 1]).DistFromStart = selectedRoad.DistFromStart + 1;
                        ((Road)_gameArea[roadPos.X, roadPos.Y + 1]).ParentRoad = selectedRoad;
                        ((Road)_gameArea[roadPos.X, roadPos.Y + 1]).NumOfVisits = selectedRoad.NumOfVisits;
                        Q.Enqueue((Road)_gameArea[roadPos.X, roadPos.Y + 1]);
                    }
                }
                else if (roadPos.Y < width - 1 && 
                    (_gameArea[roadPos.X, roadPos.Y + 1] is Residental ||   // jobbra Residental van
                    _gameArea[roadPos.X, roadPos.Y + 1] is Service ||       // jobbra Service van
                    _gameArea[roadPos.X, roadPos.Y + 1] is Industrial))     // jobbra Industrial van
                {
                    checkSurroundings(new Position(roadPos.X, roadPos.Y + 1), ignoredPos.X == -1);
                }

                if (roadPos.X < height - 1 && _gameArea[roadPos.X + 1, roadPos.Y] is Road road && (roadPos.X + 1 != ignoredPos.X || roadPos.Y != ignoredPos.Y)) // lefele Road van
                {
                    if(road.NumOfVisits < selectedRoad.NumOfVisits)  // ha még nem vizsgáltuk meg az adott Roadot
                    {
                        road.DistFromStart = selectedRoad.DistFromStart + 1;
                        road.ParentRoad = selectedRoad;
                        road.NumOfVisits = selectedRoad.NumOfVisits;
                        Q.Enqueue(road);
                    }
                }
                else if (roadPos.X < height - 1 && 
                    (_gameArea[roadPos.X + 1, roadPos.Y] is Residental ||   // lefele Residental van
                    _gameArea[roadPos.X + 1, roadPos.Y] is Service ||       // lefele Service van
                    _gameArea[roadPos.X + 1, roadPos.Y] is Industrial))     // lefele Industrial van
                {
                    checkSurroundings(new Position(roadPos.X + 1, roadPos.Y), ignoredPos.X == -1);
                }
            }
        }

        #endregion

        #region Event Methods
        private void OnGameStarted()
        {
            GameStarted?.Invoke(this, EventArgs.Empty);
        }

        private void OnFieldChanged(IArea bArea, int x, int y, int Money)
        {
            if (FieldChanged is not null)
            {
                FieldChanged(this, new FieldChangedEventArgs(bArea, x, y, Money));
            }
        }

        #endregion

        #region Persistence
        /// <summary>
        /// Játék betöltése.
        /// </summary>
        /// <param name="path">Elérési útvonal.</param>
        public async Task LoadGameAsync(String path)
        {
            if (_dataAccess == null)
                throw new InvalidOperationException("No data access is provided.");

            GameModel model = await _dataAccess.LoadAsync(path);
            GameSpeed = model.GameSpeed;
            Tax = model.Tax;
            Money = model.Money;
            Satisfaction= model.Satisfaction;
            Population = model.Population;
            Name = model.Name;
            Time = model.Time;
            _gameArea = model._gameArea;
            _startingRoad = model._startingRoad;
            
            int numberOfZone = 0;
            int numberOfPolice = 0;
            int numberOfStadium = 0;
            int numberOfHighSchool = 0;
            int numberOfUni = 0;
            int numberOfRoad = 0;
            int numberOfForest = 0;
            int currentSumTax = 0;

            for (int i = 0; i < HEIGHT; i++)
            {
                for (int j = 0; j < WIDTH; j++)
                {
                    Position pos = new Position(i, j);
                    if (i == 0 && j == 0) continue;
                    checkSurroundings(pos, true);
                    if (_gameArea[i, j] is Residental || _gameArea[i, j] is Service || _gameArea[i, j] is Industrial)
                        numberOfZone++;
                    else if (_gameArea[i, j] is Police)
                        numberOfPolice++;
                    else if (_gameArea[i, j] is Stadium)
                        numberOfStadium++;
                    else if (_gameArea[i, j] is Education ed)
                    {
                        if (ed.Level == EducationLevel.HighSchool) numberOfHighSchool++;
                        else numberOfUni++;
                    }
                    else if (_gameArea[i, j] is Road)
                        numberOfRoad++;
                    else if (_gameArea[i, j] is Forest)
                        numberOfForest++;

                    if (_gameArea[i, j] is Stadium && _gameArea[i, j + 1] is Stadium && _gameArea[i + 1, j] is Stadium)
                    {
                        if (_gameArea[i, j].Id == _gameArea[i, j + 1].Id && _gameArea[i, j].Id == _gameArea[i + 1, j].Id)
                        {
                            _gameArea[i, j].ImageId = 1;
                            OnFieldChanged(_gameArea[i, j], i, j, Money);
                            _gameArea[i, j+1].ImageId = 2;
                            OnFieldChanged(_gameArea[i, j+1], i, j+1, Money);
                            _gameArea[i+1, j].ImageId = 3;
                            OnFieldChanged(_gameArea[i+1, j], i+1, j, Money);
                            _gameArea[i+1, j + 1].ImageId = 4;
                            OnFieldChanged(_gameArea[i+1, j + 1], i+1, j + 1, Money);
                        }
                    }
                    else if (_gameArea[i, j] is Education ed)
                    {
                        if (ed.Level == EducationLevel.HighSchool)
                        {
                            if (_gameArea[i, j + 1] is Education && _gameArea[i, j].Id == _gameArea[i, j + 1].Id)
                            {
                                _gameArea[i, j].ImageId = 1;
                                OnFieldChanged(_gameArea[i, j], i, j, Money);
                                _gameArea[i, j+1].ImageId = 2;
                                OnFieldChanged(_gameArea[i, j+1], i, j+1, Money);
                            }
                        }
                        else if (_gameArea[i, j] is Education && _gameArea[i, j + 1] is Education && _gameArea[i + 1, j] is Education)
                        {
                            if (_gameArea[i, j].Id == _gameArea[i, j + 1].Id && _gameArea[i, j].Id == _gameArea[i + 1, j].Id)
                            {
                                _gameArea[i, j].ImageId = 1;
                                OnFieldChanged(_gameArea[i, j], i, j, Money);
                                _gameArea[i, j + 1].ImageId = 2;
                                OnFieldChanged(_gameArea[i, j + 1], i, j + 1, Money);
                                _gameArea[i + 1, j].ImageId = 3;
                                OnFieldChanged(_gameArea[i + 1, j], i + 1, j, Money);
                                _gameArea[i + 1, j + 1].ImageId = 4;
                                OnFieldChanged(_gameArea[i + 1, j + 1], i + 1, j + 1, Money);
                            }
                        }
                    }
                    else OnFieldChanged(_gameArea[i, j], i, j, Money);


                    if (_gameArea[i, j] is Residental res)
                    {
                        currentSumTax = currentSumTax + res.Fullness * _tax + res.HighSchoolers + res.BScCitizens * 2;
                    }

                }
 
            }
            _numberOfZones= numberOfZone;
            _numberOfPolices= numberOfPolice;
            _numberOfStadiums= numberOfStadium/4;
            _numberOfSchools= numberOfUni/4 + numberOfHighSchool/2;
            _numberOfHighSchools = numberOfHighSchool / 2;
            _numberOfUnis = numberOfUni/4;
            _numberOfRoads= numberOfRoad;
            _forests = numberOfForest;
            _currentSumTax = currentSumTax;
        }

        /// <summary>
        /// Játék mentése.
        /// </summary>
        /// <param name="path">Elérési útvonal.</param>
        public async Task SaveGameAsync(String path)
        {
            if (_dataAccess == null)
                throw new InvalidOperationException("No data access is provided.");

            await _dataAccess.SaveAsync(path, this);
        }

        #endregion
    }
}

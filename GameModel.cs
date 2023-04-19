using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace AssemblyGame.Model
{
    public class GameModel
    {

        private Int32 HEIGHT = 30;
        private Int32 WIDTH = 40;
        private IArea[,] _gameArea;  // itt tárolunk minden mezőt
        private Speed _gameSpeed;
        private Int32 _tax;

        private Int32 _money;
        private double _catastrophyProbability;

        private List<Residental> _allResidents;
        public Double Tax { get; set; }

        public IArea this[int x, int y]
        {
            get
            {
                if(x < 0 || x > _gameArea.GetLength(0))
                    throw new ArgumentOutOfRangeException("Bad column index", nameof(x));
                if (y < 0 || y > _gameArea.GetLength(1))
                    throw new ArgumentOutOfRangeException("Bad row index", nameof(y));
                return _gameArea[x, y];
            }
        }

        public Int32 Money
        {
            get => _money; 
            set => _money = value;
        }
        public Speed GameSpeed
        {
            get => _gameSpeed;
            set => _gameSpeed = value;
        }

        public event EventHandler? GameStarted;
        public event EventHandler<FieldChangedEventArgs>? FieldChanged;


        public GameModel()
        {
            _gameArea = new IArea[HEIGHT, WIDTH];
            _allResidents = new List<Residental>();
            startNewGame();
            //DispatcherTimer timer = new DispatcherTimer();
            //timer.Tick += new EventHandler(timer_tick);
        }

                

        public void startNewGame()
        {
            for (int i = 0; i < HEIGHT; i++)
            {
                for (int j = 0; j < WIDTH; j++)
                {
                    _gameArea[i, j] = new Empty();
                }
            }
            Money = 100000;
            _catastrophyProbability = 0;
            _gameSpeed = Speed.Normal;


            OnGameStarted();
        }


        /// <summary>
        /// Ezt a függvényt, akkor kell meghívni, amikor lerakunk egy új szolgáltatói, vagy ipari zónát.
        /// A játékosnak össze kell kötnie a lakossági zónákat az ipari/szolgáltatási zónákkal úttal.
        /// Azt, hogy össze vannak kötve vagy sem, azt egy bool változóban tároljuk, a residential osztályban.
        /// Ez a fv. megnézi, minden Residential zónára, hogy az új lerakott ip./szolg. zóna közelebb van-e, mint az eddigi legközelebbi (légvonalban).
        /// Ha közelebb van, akkor átállítja ezt az új zóna távolságát legközelebbinek, és ide fognak járni az emberek dolgozni.
        /// </summary>
        /// <param name="zone"></param>
        public void checkAllResidentials(Zone newZone)
        {
            foreach(Residental res_ in _allResidents)
            {
                double dist = distance(res_.Position, newZone.Position);
                if(newZone is Service)
                {
                    if (dist < res_.ClosestService)
                    {
                        res_.ClosestService = dist;
                    }
                }
                else if(newZone is Industrial)
                {
                    if(dist < res_.ClosestIndustrial) 
                    { 
                        res_.ClosestIndustrial = dist;
                    }
                }
                else
                {
                    throw new Exception(); // csak industrial vagy service zónát adjunk ennek a függvénynek
                }
            }
        }

        public void GameFieldChanged(IArea bArea, int x, int y)
        {
            Money -= bArea.Price;
            bArea.Position = new Position(x, y);
            _gameArea[x,y] = bArea;
            //aapból pos(0,0nnak van allitva és itt at kellene allitani

            OnFieldChanged(bArea, x, y);
        }

        /// <summary>
        /// Végigmegy minden residential zónán, és a következő adatokat figyelembevéve kiszámolja a satisfactiont:
        /// mennyire gazdag a város
        /// legközelebbi munkahelyek
        /// adómennyiség
        /// stb.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Int32 getSatisfaction()
        {
            throw new NotImplementedException();
        }

        private double distance(Position a, Position b)
        {
            return Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y-b.Y, 2));
        }

        private void OnGameStarted()
        {
            GameStarted?.Invoke(this, EventArgs.Empty);
        }
        private void OnFieldChanged(IArea bArea, int x, int y)
        {
            if(FieldChanged is not null)
            {
                FieldChanged(this, new FieldChangedEventArgs(bArea, x, y));
            }
        }
    }
}

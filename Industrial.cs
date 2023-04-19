using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyGame.Model
{
    public class Industrial : Zone
    {
        private Position _position;
        private Int32 _price;
        private MetropolisLevel _level;
        private Int32 _workers;
        private Int32 _maxWorkers;
        public Industrial(Position pos)
        {
            _position = pos;
            _price = 200;
            _level = MetropolisLevel.Level1;
            _workers = 0;
            _maxWorkers = 50;
        }
        public override int Size => 3;
        public override Position Position { get => _position; set { _position = value; } }
        public override int Price => _price;
        public override int AnnualPrice { get => 0; }
        public override MetropolisLevel Metropolis { get => _level; set { _level = value; } }
        public override int Fullness => Capacity() - _workers;
        public override bool IsAvailable => false;
        public override string Name => "Industrial";
        public override int Capacity()
        {
            switch (_level)
            {
                case MetropolisLevel.Level1:
                    return _maxWorkers - Fullness;
                case MetropolisLevel.Level2:
                    return _maxWorkers * 2 - Fullness;
                case MetropolisLevel.Level3:
                    return _maxWorkers * 3 - Fullness;
                default:
                    return 0;
            }
        }

        //public override int CountSatisfaction()
        //{
        //    throw new NotImplementedException();
        //}

        public override void Deploy()
        {
            throw new NotImplementedException();
        }

        //public override void IncreaseSatisfaction()
        //{
        //    throw new NotImplementedException();
        //}
        public override void Remove()
        {
            throw new NotImplementedException();
        }
    }
}

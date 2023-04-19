using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyGame.Model
{
    public class Residental : Zone
    {
        private Position _position;
        private Int32 _price;
        private MetropolisLevel _level;
        private Int32 _citizens;
        private Int32 _citizens_highSchool;
        private Int32 _citizens_Bsc;
        private Int32 _maxCitizens;
        private Int32 _joblessCitizens;

        private double _closestServiceDist;      // ezekben a változókban tárolnánk, hogy milyen közel van a legközelebbi ipari és szolgáltatási zóna légvonalban
        private double _closestIndustrialDist;   // minél kisebbek ezek az értékek annál, jobban növekedik az elégedettségi szint

        private bool _serviceAvailable;         // össze van-e kötve úttal
        private bool _industrialAvailable;

        public Residental(Position pos) {
            _position = pos;
            _price = 200;
            _level = MetropolisLevel.Level1;
            _citizens = 0;
            _citizens_highSchool = 0;
            _citizens_Bsc = 0;
            _maxCitizens = 100;
        }

        public override int Size => 3;
        public override Position Position { get => _position; set { _position = value; } }
        public override int Price { get => _price; }
        public override int AnnualPrice { get => 0; }
        public override MetropolisLevel Metropolis { get => _level; set { _level = value; } }
        public override int Fullness { get => _citizens + _citizens_highSchool + _citizens_Bsc; }
        public int Satisfaction { get => CountSatisfaction(); set => throw new NotImplementedException(); }
        public double ClosestService { get => _closestServiceDist; set { _closestServiceDist = value; } }
        public double ClosestIndustrial { get => _closestIndustrialDist; set { _closestServiceDist = value; } }
        public int Jobless { get => _joblessCitizens; set { _joblessCitizens = value; } }
        public override Boolean IsAvailable => true;
        public override string Name => "Residental";

        public override int Capacity()
        {
            switch (_level)
            {
                case MetropolisLevel.Level1:
                    return _maxCitizens - Fullness;
                case MetropolisLevel.Level2:
                    return _maxCitizens * 2 - Fullness;
                case MetropolisLevel.Level3:
                    return _maxCitizens * 3 - Fullness;
                default:
                    return 0;
            }
        }

        public int CountSatisfaction()
        {
            throw new NotImplementedException();
        }

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

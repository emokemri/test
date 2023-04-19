using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyGame.Model
{
    public class Education : Building
    {
        private Position _position;
        private Int32 _price;       // építés ára
        private Int32 _annualPrice; // fenntartás ára
        private EducationLevel _level;
        private Int32 _maxDiplomas;
        private string _name;
        public override int Size => 3;

        public override Position Position { get => _position; set => _position = value; }

        public override int Price => _price;

        public override int AnnualPrice => _annualPrice;

        public override bool IsAvailable => false;
        public EducationLevel Level => _level; 
        public override string Name => _name; 


        public Education(Position pos, EducationLevel level)
        {
            _position = pos;
            _price = 100;
            _annualPrice = 30;
            _level = level;
            if (_level == EducationLevel.HighSchool)
            {
                _name = "HighSchool";
            }
            else
            {
                _name = "University";
            }
        }

        public override int Capacity()
        {
            return _maxDiplomas;
        }

        public override void Deploy()
        {
            throw new NotImplementedException();
        }

        public override void Remove()
        {
            throw new NotImplementedException();
        }

       
    }
}

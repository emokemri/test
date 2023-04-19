using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyGame.Model
{
    public class Police : Building
    {
        private Position _position;
        private Int32 _price;       // építés ára
        private Int32 _annualPrice; // fenntartás ára
        public override int Size => 3;

        public override Position Position { get => _position; set => _position = value; }

        public override int Price => _price;

        public override int AnnualPrice => _annualPrice;

        public override bool IsAvailable => false;
        public override string Name => "Police";

        public Police(Position pos)
        {
            _position = pos;
            _price = 100;
            _annualPrice = 30;
        }

        public override int Capacity()
        {
            return 0;
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

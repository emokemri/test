using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyGame.Model
{
    public class Forest : IArea
    {
        private Position _position;
        private Int32 _price;
        private Int32 _annualPrice;
        public int Size => 1;
        public Position Position { get => _position; set => _position = value; }
        public int Price => _price;
        public int AnnualPrice => _annualPrice;
        public bool IsAvailable => false;
        public string Name => "Forest";
        public Forest(Position pos)
        {
            _position = pos;
            _annualPrice = 10;
            _price = 50;
        }

        public int Capacity()
        {
            return 0;
        }

        public void Deploy()
        {
            throw new NotImplementedException();
        }

        public void Remove()
        {
            throw new NotImplementedException();
        }
    }
}

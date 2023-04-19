using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyGame.Model
{
    public class Road : IArea
    {
        private Position _position;
        private Int32 _price;
        public Road(Position pos)
        {
            _position = pos;
            _price = 50;
        }
        public int Size => 1;

        public Position Position { get => _position; set => _position = value; }

        public int Price => _price;

        public int AnnualPrice => 10;

        public bool IsAvailable => false;
        public string Name => "Road";
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

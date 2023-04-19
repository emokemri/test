using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyGame.Model
{
    public class Empty : IArea
    {
        private int _size = 1;
        private Position _position;
        public int Size =>_size;
        public Position Position { get => _position; set => _position = value; }
        public int Price => 0;
        public int AnnualPrice => 0;
        public Boolean IsAvailable => true;
        public string Name => "Empty";
        public int Capacity()
        {
            return 0;
        }

        public void Deploy() { }

        public void IncreaseSatisfaction()
        {
            throw new NotImplementedException();
        }


        public void Remove()
        {
            throw new NotImplementedException();
        }
    }
}

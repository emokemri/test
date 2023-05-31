using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyGameModel
{
    /// <summary>
    /// Katasztrófa típusa.
    /// </summary>
    public class Catastrophy : IArea
    {
        public uint Id { get; set; }
        private Position _position;
        private int imageId = 0;
        public int Width => 1;
        public int Height => 1;
        public Position Position { get => _position; set => _position = value; }
        public int Price => 0;
        public int DeletePrice => 0;
        public int AnnualPrice => 0;
        public bool IsAvailable => true;
        public string Name => "Catastrophy";
        public string Nev => "Katasztrófa";
        public int Capacity()
        {
            return 0;
        }
        public Catastrophy(Position pos)
        {
            _position = pos;
        }

        public void IncreaseSatisfaction()
        {
            throw new NotImplementedException();
        }
        public int ImageId { get => imageId; set => imageId = value; }
        public string ImageName => "Catastrophy";
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyGame.Model
{
    public abstract class Building : IArea
    {
        public abstract int Size { get; }

        public abstract Position Position { get; set; }

        public abstract int Price { get; }

        public abstract int AnnualPrice { get; }

        public abstract bool IsAvailable { get; }
        public abstract string Name { get;}

        public abstract int Capacity();

        public abstract void Deploy();

        public abstract void Remove();
    }
}

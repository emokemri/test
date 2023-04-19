using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyGame.Model
{
    public abstract class Zone : IArea
    {
        public abstract int Size { get; }
        public abstract Position Position { get; set; }
        public abstract int Price { get; }
        public abstract int AnnualPrice { get; }
        public abstract MetropolisLevel Metropolis { get; set; }

        public abstract Int32 Fullness { get;}
        public abstract Boolean IsAvailable { get; }
        public abstract string Name { get; }
        //public abstract Int32 Satisfaction { get; set; }


        public abstract void Deploy();
        public abstract void Remove();
        public abstract Int32 Capacity();
        //public abstract void IncreaseSatisfaction();
        //public abstract int CountSatisfaction();
    }
}

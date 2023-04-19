using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyGame.Model
{
    public interface IArea
    {
        public abstract int Size { get;}
        public abstract Position Position { get; set; }
        public abstract int Price { get;}
        public abstract int AnnualPrice { get;}
        public abstract Boolean IsAvailable {  get; }
        public abstract string Name { get; }
        public abstract void Deploy();
        public abstract void Remove();
        public abstract Int32 Capacity();
        //public abstract void IncreaseSatisfaction();
    }
}

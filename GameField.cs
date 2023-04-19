using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyGame.ViewModel
{
    public class GameField : ViewModelBase
    {
        private String _whichBuilding;
        public Int32 Number { get; set; }
        public String WhichBuilding
        {
            get { return _whichBuilding; }
            set
            {
                if (_whichBuilding != value)
                {
                    _whichBuilding = value;
                    OnPropertyChanged();
                }
            }
        }
        public int X { get; set; }
        public int Y { get; set; }
        public DelegateCommand? StepGame { get; set; }
        //public GameField(string buildingType, int i, int j)
        //{
        //    _whichBuilding= buildingType;
        //    X = i; Y = j;
        //    StepGame = new DelegateCommand(_ => FieldPressed?.Invoke(this, EventArgs.Empty));
        //}
    }
}

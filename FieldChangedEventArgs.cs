namespace AssemblyGame.Model
{
    public class FieldChangedEventArgs
    {
        public IArea BArea;
        public int X;
        public int Y;

        public FieldChangedEventArgs(IArea bArea, int x, int y)
        {
            BArea = bArea;
            X = x;
            Y = y;
        }
    }
}
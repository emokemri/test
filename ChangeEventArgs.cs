using System;
using System.Windows;

namespace AssemblyGame.ViewModel
{
    public class ChangeEventArgs : RoutedEventArgs
    {
        public string SpeedMode;
        public string Building;
        public ChangeEventArgs(string speedMode)
        {
            SpeedMode = speedMode;
        }
    }
}
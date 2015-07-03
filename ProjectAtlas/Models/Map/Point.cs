using GalaSoft.MvvmLight;

namespace ProjectAtlas.Models.Map
{
    public class Point : ViewModelBase, IMapObject
    {
        private double _x;
        private double _y;

        public double X
        {
            get { return _x; }
            set
            {
                Set(() => X, ref _x, value);
            }
        }
        public double Y
        {
            get { return _y; }
            set { Set(() => Y, ref _y, value); }
        }
    }
}
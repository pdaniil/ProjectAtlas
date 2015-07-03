using GalaSoft.MvvmLight;

namespace ProjectAtlas.Models.Map
{
    public class Connector : ViewModelBase, IMapObject
    {
        private Point _startPoint;
        private Point _endPoint;

        public Point StartPoint
        {
            get { return _startPoint; }
            set { Set(() => StartPoint, ref _startPoint, value); }
        }
        public Point EndPoint
        {
            get { return _endPoint; }
            set { Set(() => EndPoint, ref _endPoint, value); }
        }

        public double X { get; set; }
        public double Y { get; set; }
    }
}
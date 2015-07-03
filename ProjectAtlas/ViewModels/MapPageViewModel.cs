using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using ProjectAtlas.Models;
using ProjectAtlas.Models.Map;
using Point = ProjectAtlas.Models.Map.Point;

namespace ProjectAtlas.ViewModels
{
    public class MapPageViewModel : ViewModelBase
    {
        #region Private members

        private CompositeCollection _collection;

        #endregion

        public BitmapImage DisplayedImage
        {
            get
            {
                return new BitmapImage(new Uri(Environment.CurrentDirectory + @"\world.jpg"));
            }
        }
        public CompositeCollection Collection
        {
            get { return _collection; }
            set { Set(() => Collection, ref _collection, value); }
        }

        public ObservableCollection<Point> Points { get; set; }
        public ObservableCollection<Connector> Connects { get; set; }

        public MapPageViewModel()
        {
            Messenger.Default.Register<Path>(this, GetPath);
        }

        private void GetPath(Path obj)
        {
            Connects = new ObservableCollection<Connector>();

            Points = new ObservableCollection<Point>(
                obj.Points.Select(x => new Point()
                    {
                        X = ((x.From.X < 0) ? Math.Abs(Math.Abs(x.From.X) - 4096) : Math.Abs(x.From.X) + 4096),
                        Y = ((x.From.Z > 0) ? Math.Abs(Math.Abs(x.From.Z) - 5632) : Math.Abs(x.From.Z) + 5632)
                    })
            );

            for(int i = 0; i < Points.Count - 1; i++)
                Connects.Add(new Connector()
                {
                    StartPoint = Points[i], 
                    EndPoint = Points[i+1]
                });

            Connects.Add(new Connector()
            {
                StartPoint = Points[Points.Count - 1], 
                EndPoint = Points[0]
            });

            Collection = new CompositeCollection()
            {
                new CollectionContainer() { Collection = Points },
                new CollectionContainer() { Collection = Connects }
            };
        }
    }
}
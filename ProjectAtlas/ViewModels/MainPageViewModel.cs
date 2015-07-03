using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using ProjectAtlas.Extensions;
using ProjectAtlas.Models;
using ProjectAtlas.Views;
using Application = System.Windows.Application;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using Path = ProjectAtlas.Models.Path;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;

namespace ProjectAtlas.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        #region PrivateMembers

        private PathCollection _pathCollection;

        private Path _selectedPath;
        private Point _selectedPoint;
        private Segment _selectedSegment;

        private readonly HotKey _hotKey;
        private ObservableCollection<Offset> _offsets;
        private Offset _selectedOffset;

        #endregion

        public PathCollection PathCollection
        {
            get { return _pathCollection; }
            set { Set(() => PathCollection, ref _pathCollection, value); }
        }
        public ObservableCollection<Offset> Offsets
        {
            get { return _offsets; }
            set { Set(() => Offsets, ref _offsets, value); }
        }

        public IEnumerable SelectedPaths { get; set; }
        public IEnumerable SelectedPoints { get; set; }
        public IEnumerable SelectedSegments { get; set; }
        public ObservableCollection<Path> Paths
        {
            get { return PathCollection.Paths; }
            set
            {
                PathCollection.Paths = value;
                RaisePropertyChanged(() => Paths);
            }
        }

        public Path SelectedPath
        {
            get { return _selectedPath; }
            set
            {
                _selectedPath = value;
                RaisePropertyChanged(()=>SelectedPath);

                Messenger.Default.Send(value);
            }
        }
        public Point SelectedPoint
        {
            get { return _selectedPoint; }
            set
            {
                _selectedPoint = value;
                RaisePropertyChanged(()=>SelectedPoint);
            }
        }
        public Segment SelectedSegment
        {
            get { return _selectedSegment; }
            set
            {
                _selectedSegment = value;
                RaisePropertyChanged(()=>SelectedSegment);
            }
        }
        public Offset SelectedOffset
        {
            get { return _selectedOffset; }
            set
            {
                _selectedOffset = value;
                RaisePropertyChanged();
            }
        }

        public ICommand LoadFromFileCommand { get; private set; }
        public ICommand SaveToFileCommand { get; private set; }

        public ICommand AddPathCommand { get; private set; }
        public ICommand DeletePathsCommand { get; private set; }
        public ICommand AddPointCommand { get; private set; }
        public ICommand DeletePointsCommand { get; private set; }
        public ICommand AddSegmentCommand { get; private set; }
        public ICommand DeleteSegmentsCommand { get; private set; }

        public ICommand OpenMapCommand { get; private set; }

        public MainPageViewModel()
        {
            LoadOffsets();

            _hotKey = new HotKey(ModifierKeys.Control, Keys.Z, Application.Current.MainWindow);
            _hotKey.HotKeyPressed +=
               key => AddPoint(Game.GetCoords(SelectedOffset, "elementclient"));

            PathCollection = new PathCollection();

            LoadFromFileCommand = new RelayCommand(LoadFromFile);
            SaveToFileCommand = new RelayCommand(SaveToFile);

            AddPathCommand = new RelayCommand(AddPath);
            AddPointCommand = new RelayCommand(AddPoint);
            AddSegmentCommand = new RelayCommand(AddSegment);

            DeletePathsCommand = new RelayCommand(DeletePaths);
            DeletePointsCommand = new RelayCommand(DeletePoints);
            DeleteSegmentsCommand = new RelayCommand(DeleteSegments);

            OpenMapCommand = new RelayCommand(OpenMapExecute);
        }

        private void AddPath()
        {
            Paths.Add(new Path()
            {
                Points = new ObservableCollection<Point>(),
                Segments = new ObservableCollection<Segment>()
            });
        }
        private void AddPoint()
        {
            SelectedPath.Points.Add(new Point()
            {
                From = new Coordinates(),
                Direction = new Coordinates()
            });
        }
        private void AddPoint(Point point)
        {
            SelectedPath.Points.Add(point);
        }
        private void AddSegment()
        {
            SelectedPath.Segments.Add(new Segment());
        }

        private void DeletePaths()
        {
            foreach (var selectedPath in SelectedPaths)
                Paths.Remove(selectedPath as Path);
        }
        private void DeletePoints()
        {
            foreach (var selectedPoint in SelectedPoints)
                SelectedPath.Points.Remove(selectedPoint as Point);
        }
        private void DeleteSegments()
        {
            foreach (var selectedSegment in SelectedSegments)
                SelectedPath.Segments.Remove(selectedSegment as Segment);
        }

        private void LoadFromFile()
        {
            var opf = new OpenFileDialog();

            var showDialog = opf.ShowDialog();
            if (showDialog != null && (bool)showDialog)
            {
                using (var reader = new BinaryReader(new FileStream(opf.FileName, FileMode.Open, FileAccess.Read)))
                {
                    PathCollection = reader.ReadModel<PathCollection>();

                    RaisePropertyChanged(() => Paths);
                }
            }
        }
        private void SaveToFile()
        {
            var sfd = new SaveFileDialog();
            var showDialog = sfd.ShowDialog();

            if (showDialog != null && (bool)showDialog)
            {
                using (var writer = new BinaryWriter(new FileStream(sfd.FileName, FileMode.Create, FileAccess.Write)))
                {
                    writer.WriteModel(PathCollection);
                }
            }
        }

        private void LoadOffsets()
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\offsets.txt"))
            {
                _offsets = new ObservableCollection<Offset>();

                try
                {
                    var sr = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + @"\offsets.txt", Encoding.Unicode);

                    while (!sr.EndOfStream)
                    {
                        var nameStr = sr.ReadLine();

                        if (nameStr.StartsWith("Name="))
                        {
                            var offset = new Offset();

                            offset.Name = nameStr.Substring(5);

                            var value = sr.ReadLine().Split(' ');

                            offset.BaseChain = new int[value.Length];
                            for (int h = 0; h < value.Length; h++)
                            {
                                offset.BaseChain[h] = int.Parse(value[h], NumberStyles.HexNumber);
                            }

                            offset.DirX = int.Parse(sr.ReadLine(), NumberStyles.HexNumber);
                            offset.DirY = int.Parse(sr.ReadLine(), NumberStyles.HexNumber);
                            offset.DirZ = int.Parse(sr.ReadLine(), NumberStyles.HexNumber);
                            offset.PosX = int.Parse(sr.ReadLine(), NumberStyles.HexNumber);
                            offset.PosY = int.Parse(sr.ReadLine(), NumberStyles.HexNumber);
                            offset.PosZ = int.Parse(sr.ReadLine(), NumberStyles.HexNumber);

                            Offsets.Add(offset);
                        }
                    }
                }
                catch (Exception) { }
            }
        }

        private void OpenMapExecute()
        {
            var window = new MapPageView();
            window.Show();
        }
    }
}
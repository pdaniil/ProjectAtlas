using System.Collections.ObjectModel;
using System.IO;
using ProjectAtlas.Extensions;

namespace ProjectAtlas.Models
{
    public class Path : IBinaryModel
    {
        public int Unk { get; set; }
        public int Id { get; set; }

        public int Flag1 { get; set; }
        public int Flag2 { get; set; }

        public int PointsCount { get; set; }
        public ObservableCollection<Point> Points { get; set; }

        public int SegmentsCount { get; set; }
        public ObservableCollection<Segment> Segments { get; set; }

        #region IBinaryModel implementation

        public void ReadInternal(BinaryReader reader)
        {
            Points = new ObservableCollection<Point>();
            Segments = new ObservableCollection<Segment>();

            Unk = reader.ReadInt32();
            Id = reader.ReadInt32();
            Flag1 = reader.ReadInt32();
            Flag2 = reader.ReadInt32();

            PointsCount = reader.ReadInt32();
            for (int i = 0; i < PointsCount; i++)
                Points.Add(reader.ReadModel<Point>());

            SegmentsCount = reader.ReadInt32();
            for (int i = 0; i < SegmentsCount; i++)
                Segments.Add(reader.ReadModel<Segment>());
                
        }

        public void WriteInternal(BinaryWriter writer)
        {
            writer.Write(Unk);
            writer.Write(Id);
            writer.Write(Flag1);
            writer.Write(Flag2);

            writer.Write(PointsCount);
            foreach (var point in Points)
                writer.WriteModel(point);


            writer.Write(SegmentsCount);
            foreach (var segment in Segments)
                writer.WriteModel(segment);
        }

        #endregion
    }
}

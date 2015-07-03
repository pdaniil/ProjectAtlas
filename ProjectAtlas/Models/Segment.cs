using System.IO;
using ProjectAtlas.Extensions;

namespace ProjectAtlas.Models
{
    public class Segment : IBinaryModel
    {
        public Coordinates AnchorHead { get; set; }
        public Coordinates AnchorTail { get; set; }
        public int HeadPoint { get; set; }
        public int TailPoint { get; set; }
        public float SeqLength { get; set; }

        #region IBinaryModel implementation

        public void ReadInternal(BinaryReader reader)
        {
            AnchorHead = reader.ReadModel<Coordinates>();
            AnchorTail = reader.ReadModel<Coordinates>();
            HeadPoint = reader.ReadInt32();
            TailPoint = reader.ReadInt32();
            SeqLength = reader.ReadSingle();
        }

        public void WriteInternal(BinaryWriter writer)
        {
            writer.WriteModel(AnchorHead);
            writer.WriteModel(AnchorTail);
            writer.Write(HeadPoint);
            writer.Write(TailPoint);
            writer.Write(SeqLength);
        }

        #endregion
    }
}
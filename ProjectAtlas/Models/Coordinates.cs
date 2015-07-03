using System.IO;
using ProjectAtlas.Extensions;

namespace ProjectAtlas.Models
{
    public class Coordinates : IBinaryModel
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        #region IBinaryModel implementation

        public void ReadInternal(BinaryReader reader)
        {
            X = reader.ReadSingle();
            Y = reader.ReadSingle();
            Z = reader.ReadSingle();
        }

        public void WriteInternal(BinaryWriter writer)
        {
            writer.Write(X);
            writer.Write(Y);
            writer.Write(Z);
        }

        #endregion
    }
}
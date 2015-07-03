using System.IO;
using ProjectAtlas.Extensions;

namespace ProjectAtlas.Models
{
    public class Point : IBinaryModel
    {
        public Coordinates From { get; set; }
        public Coordinates Direction { get; set; }

        #region IBinaryModel implementation

        public void ReadInternal(BinaryReader reader)
        {
            From = reader.ReadModel<Coordinates>();
            Direction = reader.ReadModel<Coordinates>();
        }

        public void WriteInternal(BinaryWriter writer)
        {
            writer.WriteModel(From);
            writer.WriteModel(Direction);
        }

        #endregion
    }
}
using System.Collections.ObjectModel;
using System.IO;
using ProjectAtlas.Extensions;

namespace ProjectAtlas.Models
{
    public class PathCollection : IBinaryModel
    {
        public int Version { get; set; }
        public int Amount { get; set; }

        public ObservableCollection<Path> Paths { get; set; }


        #region IBinaryModel implementation

        public void ReadInternal(BinaryReader reader)
        {
            Paths = new ObservableCollection<Path>();

            Version = reader.ReadInt32();
            Amount = reader.ReadInt32();

            for (int i = 0; i < Amount; i++)
                Paths.Add(reader.ReadModel<Path>());
        }

        public void WriteInternal(BinaryWriter writer)
        {
            writer.Write(Version);
            writer.Write(Amount);

            foreach (var path in Paths)
                writer.WriteModel(path);
        }

        #endregion
    }
}
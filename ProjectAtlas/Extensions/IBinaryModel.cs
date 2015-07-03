using System.IO;

namespace ProjectAtlas.Extensions
{
    public interface IBinaryModel
    {
        void ReadInternal(BinaryReader reader);
        void WriteInternal(BinaryWriter writer);
    }
}
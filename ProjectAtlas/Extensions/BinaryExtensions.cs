using System.IO;

namespace ProjectAtlas.Extensions
{
    public static class BinaryExtensions
    {
        internal static TModel ReadModel<TModel>(this BinaryReader reader, int version = 0)
            where TModel : IBinaryModel, new()
        {
            var model = new TModel();
            model.ReadInternal(reader);
            return model;
        }

        internal static void WriteModel<TModel>(this BinaryWriter writer, TModel model, int version = 0)
            where TModel : IBinaryModel
        {
            model.WriteInternal(writer);
        } 
    }
}
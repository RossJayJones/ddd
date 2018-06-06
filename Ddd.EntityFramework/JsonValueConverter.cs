using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Ddd.EntityFramework
{
    public class JsonValueConverter<TModel> : ValueConverter<TModel, string>
    {
        public JsonValueConverter()
           : base(data => ToJson(data), data => ToModel(data))
        {
            
        }

        static string ToJson(TModel model)
        {
            return string.Empty;
        }

        static TModel ToModel(string json)
        {
            return default(TModel);
        }
    }
}

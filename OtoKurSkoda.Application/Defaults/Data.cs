using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace OtoKurSkoda.Application.Defaults
{
    [JsonObject]
    public class Data<T>
    {
        [JsonPropertyName("Items")]
        public T Items
        {
            get;
            set;
        }

        [JsonPropertyName("Count")]
        public int Count
        {
            get;
            set;
        }

        public Data(T items, int count)
        {
            Items = items;
            Count = count;
        }
    }
}

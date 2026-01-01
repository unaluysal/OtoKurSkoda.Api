using System.Text.Json.Serialization;

namespace OtoKurSkoda.Application.Defaults
{
    public class Data<T>
    {
        [JsonPropertyName("items")]
        public T Items { get; set; }

        [JsonPropertyName("count")]
        public int Count { get; set; }

        public Data(T items, int count)
        {
            Items = items;
            Count = count;
        }
    }
}

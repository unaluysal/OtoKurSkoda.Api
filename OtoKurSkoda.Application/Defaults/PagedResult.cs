using System.Text.Json.Serialization;

namespace OtoKurSkoda.Application.Defaults
{
    public class PagedResult<T>
    {
        [JsonPropertyName("Items")]
        public List<T> Items { get; set; }

        [JsonPropertyName("TotalCount")]
        public int TotalCount { get; set; }

        [JsonPropertyName("PageNumber")]
        public int PageNumber { get; set; }

        [JsonPropertyName("PageSize")]
        public int PageSize { get; set; }

        [JsonPropertyName("TotalPages")]
        public int TotalPages { get; set; }

        public PagedResult()
        {
            Items = new List<T>();
        }
    }
}

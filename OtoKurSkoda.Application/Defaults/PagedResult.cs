using System.Text.Json.Serialization;

namespace OtoKurSkoda.Application.Defaults
{
    public class PagedResult<T>
    {
        [JsonPropertyName("items")]
        public List<T> Items { get; set; }

        [JsonPropertyName("totalCount")]
        public int TotalCount { get; set; }

        [JsonPropertyName("pageNumber")]
        public int PageNumber { get; set; }

        [JsonPropertyName("pageSize")]
        public int PageSize { get; set; }

        [JsonPropertyName("totalPages")]
        public int TotalPages { get; set; }

        public PagedResult()
        {
            Items = new List<T>();
        }
    }
}

using System.Text.Json.Serialization;

namespace OtoKurSkoda.Application.Defaults
{
    public class PagedDataResult<T> : ServiceResult
    {
        [JsonPropertyName("data")]
        public PagedResult<T> Data { get; set; }

        public PagedDataResult(PagedResult<T> data, bool status, string messageCode, string message, ResultStatusCode resultStatusCode)
            : base(status, messageCode, message, resultStatusCode)
        {
            Data = data;
        }
    }
}

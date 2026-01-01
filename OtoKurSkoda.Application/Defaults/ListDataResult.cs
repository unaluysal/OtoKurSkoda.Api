using System.Collections;
using System.Text.Json.Serialization;

namespace OtoKurSkoda.Application.Defaults
{
    public class ListDataResult<T> : ServiceResult where T : ICollection
    {
        [JsonPropertyName("data")]
        public Data<T> Data { get; set; }

        public ListDataResult()
            : base(status: true, "", "", ResultStatusCode.Success)
        {
        }

        public ListDataResult(T data, bool status, string messageCode, string message, ResultStatusCode resultStatusCode)
            : base(status, messageCode, message, resultStatusCode)
        {
            Data = new Data<T>(data, data.Count);
        }
    }
}

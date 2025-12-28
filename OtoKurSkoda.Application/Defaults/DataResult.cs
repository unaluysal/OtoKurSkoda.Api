using System.Text.Json.Serialization;

namespace OtoKurSkoda.Application.Defaults
{
    public class DataResult<T> : ServiceResult
    {
        [JsonPropertyName("Data")]
        public T Data
        {
            get;
            set;
        }

        public DataResult()
            : base(status: true, "", "", ResultStatusCode.Success)
        {
        }

        public DataResult(T data, bool status, string messageCode, string message, ResultStatusCode resultStatusCode)
            : base(status, messageCode, message, resultStatusCode)
        {
            Data = data;
        }
    }
}

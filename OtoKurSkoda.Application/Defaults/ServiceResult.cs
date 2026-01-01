using System.Text.Json.Serialization;

namespace OtoKurSkoda.Application.Defaults
{
    public abstract class ServiceResult
    {
        [JsonPropertyName("status")]
        public bool Status { get; set; }

        [JsonPropertyName("statusCode")]
        public ResultStatusCode StatusCode { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("messageCode")]
        public string MessageCode { get; set; }

        public ServiceResult(bool status, string messagecode, string message, ResultStatusCode resultStatusCode)
        {
            Status = status;
            Message = message;
            MessageCode = messagecode;
            StatusCode = resultStatusCode;
        }
    }
}

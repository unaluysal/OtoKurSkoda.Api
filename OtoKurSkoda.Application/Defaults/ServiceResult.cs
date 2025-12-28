using System.Text.Json.Serialization;

namespace OtoKurSkoda.Application.Defaults
{
    public abstract class ServiceResult
    {
        [JsonPropertyName("Status")]
        public bool Status { get; set; }

        [JsonPropertyName("StatusCode")]
        public ResultStatusCode StatusCode { get; set; }


        [JsonPropertyName("Message")]
        public string Message { get; set; }

        [JsonPropertyName("MessageCode")]
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

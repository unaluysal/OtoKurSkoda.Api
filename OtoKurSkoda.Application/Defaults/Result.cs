namespace OtoKurSkoda.Application.Defaults
{
    public class Result : ServiceResult
    {
        public Result(bool status, string messageCode, string message, ResultStatusCode resultStatusCode)
            : base(status, messageCode, message, resultStatusCode)
        {
        }
    }
}

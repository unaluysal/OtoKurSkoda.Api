namespace OtoKurSkoda.Application.Defaults
{
    public class BaseDto
    {

        public Guid Id { get; set; }

        public DateTime CreateTime { get; set; }

        public Guid CreateUserId { get; set; }

        public DateTime? UpdateTime { get; set; }

        public Guid? UpdateUserId { get; set; }

        public Guid TenatId { get; set; }

        public bool Status { get; set; }
    }
}

namespace OtoKurSkoda.Application.Dtos
{
    public class UpdateRoleRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}

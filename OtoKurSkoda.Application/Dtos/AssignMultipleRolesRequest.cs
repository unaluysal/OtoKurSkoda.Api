namespace OtoKurSkoda.Application.Dtos
{
    public class AssignMultipleRolesRequest
    {
        public Guid RoleGroupId { get; set; }
        public List<Guid> RoleIds { get; set; } = new();
    }
}

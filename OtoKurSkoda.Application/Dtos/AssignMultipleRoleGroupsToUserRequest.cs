namespace OtoKurSkoda.Application.Dtos
{
    public class AssignMultipleRoleGroupsToUserRequest
    {
        public Guid UserId { get; set; }
        public List<Guid> RoleGroupIds { get; set; } = new();
    }
}

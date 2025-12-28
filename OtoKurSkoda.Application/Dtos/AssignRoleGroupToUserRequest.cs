namespace OtoKurSkoda.Application.Dtos
{
    public class AssignRoleGroupToUserRequest
    {
        public Guid UserId { get; set; }
        public Guid RoleGroupId { get; set; }
    }
}

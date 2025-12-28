namespace OtoKurSkoda.Application.Dtos
{
    public class UserPermissionsDto
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public List<string> RoleGroups { get; set; } = new();
        public List<string> Permissions { get; set; } = new();
    }
}

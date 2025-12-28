namespace OtoKurSkoda.Application.Dtos
{
    public class UpdateUserRequest
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string? Password { get; set; }  // Null ise değişmeyecek
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool PhoneConfirmed { get; set; }
        public List<Guid> RoleGroupIds { get; set; } = new();
    }
}

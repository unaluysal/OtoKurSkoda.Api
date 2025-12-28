using OtoKurSkoda.Application.Defaults;

namespace OtoKurSkoda.Application.Dtos
{
    public class UserDto : BaseDto
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public bool EmailConfirmed { get; set; }
        public bool PhoneConfirmed { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public List<string> Roles { get; set; } = new();
    }
}

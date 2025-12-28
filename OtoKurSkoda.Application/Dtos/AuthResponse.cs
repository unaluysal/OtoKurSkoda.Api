namespace OtoKurSkoda.Application.Dtos
{
    public class AuthResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime AccessTokenExpiration { get; set; }
        public UserDto User { get; set; }
    }
}

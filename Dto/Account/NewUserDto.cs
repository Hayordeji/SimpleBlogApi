namespace API.Dto.Account
{
    public class NewUserDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string JWTtoken { get; set; }
    }
}

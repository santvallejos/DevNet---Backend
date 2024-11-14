namespace DevNet_WebAPI.Infrastructure.DTO
{
    public class CreateUserDto
    {
        public Guid RoleId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ProfileImageUrl { get; set; }
    }
}

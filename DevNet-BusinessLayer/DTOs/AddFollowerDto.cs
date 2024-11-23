namespace DevNet_WebAPI.Infrastructure.DTO
{
    public class AddFollowerDto
    {
        public Guid FollowerId { get; set; }
        public Guid FollowedId { get; set; }
    }
}

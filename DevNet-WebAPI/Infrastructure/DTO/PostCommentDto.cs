namespace DevNet_WebAPI.Infrastructure.DTO
{
    public class PostCommentDto
    {
        public Guid PostId { get; set; }
        public Guid UserId { get; set; }
        public string Text { get; set; }
    }
}

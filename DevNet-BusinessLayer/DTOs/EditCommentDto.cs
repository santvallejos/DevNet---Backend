namespace DevNet_WebAPI.Infrastructure.DTO
{
    public class EditCommentDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Text { get; set; }
    }
}

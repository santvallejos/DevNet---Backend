namespace DevNet_WebAPI.Infrastructure.DTO
{
    public class NewPostDto
    {
        public Guid UserId { get; set; }
        public string Text { get; set; }
        public string MediaUrl { get; set; }
    }
}

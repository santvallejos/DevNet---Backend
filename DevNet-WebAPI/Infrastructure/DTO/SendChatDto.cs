namespace DevNet_WebAPI.Infrastructure.DTO
{
    public class SendChatDto
    {
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public string Text { get; set; }
    }
}

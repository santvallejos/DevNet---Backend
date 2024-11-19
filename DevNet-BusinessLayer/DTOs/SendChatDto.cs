namespace DevNet_BusinessLayer.DTOs
{
    public class SendChatDto
    {
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public string Text { get; set; }
    }
}

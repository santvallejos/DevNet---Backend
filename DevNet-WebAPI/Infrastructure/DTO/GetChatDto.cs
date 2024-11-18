namespace DevNet_WebAPI.Infrastructure.DTO
{
    public class GetChatDto
    {
        public Guid Id { get; set; }
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
    }
}

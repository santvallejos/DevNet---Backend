namespace DevNet_WebAPI.Infrastructure.DTO
{
    public class SendNotificationDto
    {
        public Guid UserId { get; set; }
        public string Type { get; set; }
        public string Text { get; set; }
        public Guid RelatedUserId { get; set; }
        public Guid PostId { get; set; }
    }
}

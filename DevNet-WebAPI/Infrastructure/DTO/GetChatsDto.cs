namespace DevNet_WebAPI.Infrastructure.DTO
{
    public class GetChatsDto
    {
        public Guid UserId { get; set; }
        public Guid RelatedUserId { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}

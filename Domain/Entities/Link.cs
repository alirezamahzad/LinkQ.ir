using Domain.Enums;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public class Link
    {
        public int Id { get; set; }
        public required Url OriginalUrl { get; set; }
        public required string ShortUrl { get; set; }
        public DateTime RegDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public int UserId { get; set; }
        public string? HashedPassword { get; set; }
        public LinkStatus LinkStatus { get; set; }
    }
}

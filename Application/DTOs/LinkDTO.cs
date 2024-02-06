using Domain.Entities;
using Domain.Enums;

namespace Application.DTOs
{
    public class LinkDTO
    {
        public LinkDTO(string originalUrl, string shortUrl, DateTime? expireDate, LinkStatus status)
        {
            Status = status;
            OriginalUrl = originalUrl;
            ShortUrl = shortUrl;
            ExpireDate = expireDate;
        }
        public string OriginalUrl { get; set; }
        public string ShortUrl { get; set; }
        public DateTime? ExpireDate { get; set; }
        public LinkStatus Status { get; set; }
    }
}

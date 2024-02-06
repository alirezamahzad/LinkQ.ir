using Domain.Entities;
using Domain.Enums;

namespace Application.DTOs
{
    public class LinkDTO
    {
        public LinkDTO(string originalUrl, string shortUrl, DateTime? expireDate, Link.Status status, IEnumerable<int>? statistic)
        {
            Status = status;
            OriginalUrl = originalUrl;
            ShortUrl = shortUrl;
            ExpireDate = expireDate;
            Statistic = statistic;
        }
        public string OriginalUrl { get; set; }
        public string ShortUrl { get; set; }
        public DateTime? ExpireDate { get; set; }
        public LinkStatus Status { get; set; }
        public IEnumerable<int>? Statistic { get; }
    }
}

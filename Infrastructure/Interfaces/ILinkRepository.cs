using Domain.Entities;

namespace Infrastructure.Interfaces
{
    public interface ILinkRepository
    {
        Task<int> NumberOfLinksCreated(int userId);
        Task Delete(int id);
        Task<IEnumerable<Link>> Find(string Search);
        Task<string> GetUrl(string shortUrl);
        Task<Link>? GetLink(string shortUrl);
    }
}

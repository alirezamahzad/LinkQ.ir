using Domain.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class LinkRepository: ILinkRepository
    {
        private readonly DbEntities _context;
        public LinkRepository(DbEntities context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<int> NumberOfLinksCreated(int userId)
        {
            var q=await _context.Links.Where(i=>i.UserId == userId).ToListAsync();
            return q.Count();
        }
        public async Task Delete(int id)
        {
            var q= await _context.Links.Where(_i => _i.Id == id).FirstOrDefaultAsync();
            q.LinkStatus = Domain.Enums.LinkStatus.Deleted;
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Link>> Find(string Search)
        {
            return await _context.Links.Where(i => i.OriginalUrl.Value.Contains(Search) || i.ShortUrl.Contains(Search)).ToListAsync();
        }
        public async Task<string> GetUrl(string shortUrl)
        {
            var url = await _context.Links.FirstOrDefaultAsync(i => i.ShortUrl == shortUrl);
            if (url is not null)
                return url.OriginalUrl.Value;
            return string.Empty;
        }
        public async Task<Link>? GetLink(string shortUrl)
        {
            var link = await _context.Links.FirstOrDefaultAsync(i => i.ShortUrl == shortUrl);
            if (link is not null)
                return link;
            return null;
        }
    }
}

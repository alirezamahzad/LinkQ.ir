using Domain.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class DuplicationRepository<T> : IDuplicationRepository<T> where T : class
    {
        private readonly DbEntities _context;

        public DuplicationRepository(DbEntities context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> IsDuplicate(T entity, int? id)
        {
            return entity switch
            {
                Link link => await IsDuplicateLink(link, id),
                User user => await IsDuplicateUser(user, id),
                _ => false
            };
        }

        private async Task<bool> IsDuplicateLink(Link link, int? id)
        {
            var query = _context.Links
                .Where(l => l.LinkStatus == Domain.Enums.LinkStatus.Active && l.ShortUrl == link.ShortUrl);

            if (id.HasValue)
                query = query.Where(l => l.Id != id);

            return await query.AnyAsync();
        }

        private async Task<bool> IsDuplicateUser(User user, int? id)
        {
            var query = _context.Users
                .Where(u => u.UserStatus == User.Status.Active && u.Email == user.Email);

            if (id.HasValue)
                query = query.Where(u => u.Id != id);

            return await query.AnyAsync();
        }
    }
}

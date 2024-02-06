using Domain.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class VisitRepository : IVisitRepository
    {
        private readonly DbEntities _context;
        private readonly ILinkRepository _linkRepository;

        public VisitRepository(DbEntities context, ILinkRepository linkRepository)
        {
            _context = context;
            _linkRepository = linkRepository;
        }

        public async Task<IEnumerable<int>>? GetStatistic(string shortUrl)
        {
            try
            {
                var link = await _linkRepository.GetLink(shortUrl);
                if (link == null)
                    return null;

                var today = DateTime.Today;

                var dailyVisitorsCount = await GetVisitorsCount(link.Id, today);
                var totalVisitorsCount = await GetVisitorsCount(link.Id);

                var dailyViewsCount = await GetViewsCount(link.Id, today);
                var totalViewsCount = await GetViewsCount(link.Id);

                return new int[] { totalVisitorsCount, dailyVisitorsCount, totalViewsCount, dailyViewsCount };
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                Console.WriteLine($"An error occurred while retrieving visit statistics: {ex.Message}");
                return null;
            }
        }

        private async Task<int> GetVisitorsCount(int linkId, DateTime? date = null)
        {
            IQueryable<Visit> visitsQuery = _context.Visits.Where(v => v.UrlId == linkId);

            if (date.HasValue)
                visitsQuery = visitsQuery.Where(v => v.RegDate.Date == date.Value.Date);

            return await visitsQuery.Select(v => v.IpAddress).Distinct().CountAsync();
        }

        private async Task<int> GetViewsCount(int linkId, DateTime? date = null)
        {
            IQueryable<Visit> visitsQuery = _context.Visits.Where(v => v.UrlId == linkId);

            if (date.HasValue)
                visitsQuery = visitsQuery.Where(v => v.RegDate.Date == date.Value.Date);

            return await visitsQuery.CountAsync();
        }
    }
}

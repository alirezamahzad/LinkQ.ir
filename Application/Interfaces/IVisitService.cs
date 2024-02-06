using Domain.Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IVisitService
    {
        Task<OperationResult> Add(Visit entity);
        Task<IEnumerable<int>>? GetStatistic(string shortUrl);
    }
}

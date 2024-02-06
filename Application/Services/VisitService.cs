using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class VisitService : IVisitService
    {
        private readonly CrudRepository<Visit> _repository;
        private readonly IVisitRepository _visitRepository;
        private readonly ILogger _logger;
        public VisitService(CrudRepository<Visit> repository, IVisitRepository visitRepository, ILogger logger)
        {
            _repository = repository;
            _visitRepository = visitRepository;
            _logger = logger;
        }
        public async Task<OperationResult> Add(Visit entity)
        {
            try
            {
                return await _repository.Add(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a visit");
                return await OperationResult.CreateAsync(false, HttpStatusCode.InternalServerError);
            }
        }
        public async Task<IEnumerable<int>>? GetStatistic(string shortUrl)
        {
            try
            {
                return await _visitRepository.GetStatistic(shortUrl);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while receiving statistic");
                throw;
            }
        }
    }
}

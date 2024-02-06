using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure;
using Infrastructure.Interfaces;
using Infrastructure.Security;
using Microsoft.Extensions.Logging;
using Shared;

namespace Application.Services
{
    public class LinkService : ILinkService
    {
        private readonly ICrudRepository<Link> _repository;
        private readonly ILinkRepository _linkRepository;
        private readonly IVisitService _visitService;
        private readonly ILogger _logger;
        public LinkService(ICrudRepository<Link> repository, ILinkRepository linkRepository, IVisitService visitService, ILogger logger)
        {
            _repository = repository;
            _linkRepository = linkRepository;
            _visitService = visitService;
            _logger = logger;
        }
        public async Task<OperationResult> Add(Link link)
        {
            try
            {
                var linksCreated = await _linkRepository.NumberOfLinksCreated(link.UserId);
                if (linksCreated < 20)
                {
                    return await _repository.Add(link);
                }
                return await OperationResult.CreateAsync(false, HttpStatusCode.NotAcceptable);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a link.");
                return await OperationResult.CreateAsync(false, HttpStatusCode.InternalServerError);
            }
        }
        public async Task<OperationResult> Update(int id, Link link)
        {
            try
            {
                return await _repository.Update(id, link);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating a link.");
                return await OperationResult.CreateAsync(false, HttpStatusCode.InternalServerError);
            }
        }
        public async Task Delete(int id)
        {
            try
            {
                await _linkRepository.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting a link.");
            }
        }
        public async Task<IEnumerable<LinkDTO>> GetAll(int userId)
        {
            try
            {
                var list = new List<LinkDTO>();
                var linksAsync = await _repository.GetAll();
                var links = linksAsync.Where(i => i.UserId == userId && i.LinkStatus != Domain.Enums.LinkStatus.Deleted).OrderByDescending(i => i.RegDate);
                foreach (var link in links)
                {
                    list.Add(new LinkDTO(link.OriginalUrl.Value, link.ShortUrl, link.ExpireDate, link.LinkStatus));
                }
                return list;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting list of all of the links.");
                throw;
            }
        }
        public async Task<LinkDTO> GetLink(int id)
        {
            try
            {
                var link = await _repository.GetById(id);
                return new LinkDTO(link.OriginalUrl.Value, link.ShortUrl, link.ExpireDate, link.LinkStatus);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting a link. GetLink(id)");
                throw;
            }
        }
        public async Task<IEnumerable<LinkDTO>> Find(string search)
        {
            try
            {
                var links = await _linkRepository.Find(search);
                var list = new List<LinkDTO>();
                foreach (var link in links)
                {
                    list.Add(new LinkDTO(link.OriginalUrl.Value, link.ShortUrl, link.ExpireDate, link.LinkStatus));
                }
                return list;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while finding a link.");
                throw;
            }
        }
        public async Task<OperationResult> SetPassword(int id, string? newPassword)
        {
            try
            {
                var link = await _repository.GetById(id);
                if (link != null)
                {
                    var newLink = (new Link()
                    {
                        OriginalUrl = link.OriginalUrl,
                        ShortUrl = link.ShortUrl,
                        ExpireDate = link.ExpireDate,
                        LinkStatus = link.LinkStatus,
                        RegDate = link.RegDate,
                        UserId = link.UserId,
                        HashedPassword = null
                    });
                    if (newPassword != null)
                        newLink.HashedPassword = PasswordService.HashPasswordWithoutSalt(newPassword);
                    return await _repository.Update(id, newLink);
                }
                return await OperationResult.CreateAsync(false, HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while setting a password for a link({id}).");
                return await OperationResult.CreateAsync(false, HttpStatusCode.InternalServerError);
            }
        }
        public async Task<string> GetUrl(string shortUrl)
        {
            try
            {
                return await _linkRepository.GetUrl(shortUrl);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting url of a link.");
                throw;
            }
        }
    }
}

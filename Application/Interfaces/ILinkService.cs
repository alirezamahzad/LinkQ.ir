using Application.DTOs;
using Domain.DTO;
using Domain.Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ILinkService 
    {
        Task<OperationResult> Add(Link link);
        Task<OperationResult> Update(int id, Link link);
        Task Delete(int id);
        Task<IEnumerable<LinkDTO>> GetAll(int userId);
        Task<LinkDTO> GetLink(int id);
        Task<IEnumerable<LinkDTO>> Find(string search);
        Task<OperationResult> SetPassword(int id, string? newPassword);
        Task<string> GetUrl(string shortUrl);
    }
}

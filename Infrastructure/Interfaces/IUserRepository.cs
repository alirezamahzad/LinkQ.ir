using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetByEmail(string email);
        Task<OperationResult> Login(string email, string password);
        Task<int> GetLastId();
        Task<OperationResult> ResetPasswordRequest(string email, int resetCode);
        Task<OperationResult> ResetPasswordRequestVerify(string email, int enteredCode);
    }
}

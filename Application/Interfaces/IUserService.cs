using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<OperationResult> Register(string email, string password);
        Task<OperationResult> Login(string email, string password);
        Task<OperationResult> ResetPasswordRequest(string email);
        Task<OperationResult> ResetPasswordVerify(string email, int resetCode);
        Task<OperationResult> ResetPassword(string email, string newPassword);
    }
}

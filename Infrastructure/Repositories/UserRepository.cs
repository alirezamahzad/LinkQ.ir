using Domain.Entities;
using Domain.ValueObjects;
using Infrastructure.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared;
using System.Net;

namespace Infrastructure.Repositories
{
    public class UserRepository:IUserRepository
    {
        private readonly DbEntities _context;
        private readonly ILogger _logger;
        public UserRepository(DbEntities context,ILogger logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<User> GetByEmail(string email)
        {
            try
            {
                return await _context.Users.FirstOrDefaultAsync(i => i.Email == new Email(email));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting a user by email from the database");
                throw;
            }
        }
        public async Task<OperationResult> Login(string email,string password)
        {
            try
            {
                var user = await GetByEmail(email)!;
                if (user != null)
                {
                    if (user.HashedPassword == password)
                        return await OperationResult.CreateAsync(true, HttpStatusCode.Accepted);
                }
                return await OperationResult.CreateAsync(false, HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while loging a user in");
                return await OperationResult.CreateAsync(false, HttpStatusCode.InternalServerError);
            }
        }
        public async Task<int> GetLastId()
        {
            try
            {
                var lastRecord = await _context.Users.LastAsync();
                return lastRecord.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the last user id");
                throw;
            }
        }
        public async Task<OperationResult> ResetPasswordRequest(string email,int resetCode)
        {
            try
            {
                var user = GetByEmail(email);
                if (user != null)
                {
                    var request = new PasswordResetRequest(user.Id, resetCode, DateTime.Now.AddHours(1));
                    await _context.PasswordResetRequests.AddAsync(request);
                    await _context.SaveChangesAsync(true);
                    return await OperationResult.CreateAsync(true, HttpStatusCode.OK);
                }
                return await OperationResult.CreateAsync(false, HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while trying to store resetpassword request to the database");
                throw;
            }
        }
        public async Task<OperationResult> ResetPasswordRequestVerify(string email, int enteredCode)
        {
            try
            {
                var user = GetByEmail(email);
                if (user != null)
                {
                    var resetRequest = _context.PasswordResetRequests.Last(i => i.UserId == user.Id);
                    if (resetRequest != null)
                    {
                        if (resetRequest.Expiration > DateTime.Now)
                        {
                            if (resetRequest.ResetCode == enteredCode)
                                return await OperationResult.CreateAsync(true, HttpStatusCode.Accepted);
                            else return await OperationResult.CreateAsync(false, HttpStatusCode.NotAcceptable);
                        }
                        else return await OperationResult.CreateAsync(true, HttpStatusCode.RequestTimeout);
                    }
                    return await OperationResult.CreateAsync(true, HttpStatusCode.BadRequest);
                }
                return await OperationResult.CreateAsync(true, HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while trying to verify resetpassword request from the database");
                throw;
            }
        }
    }
}

using Application.Interfaces;
using Domain.Entities;
using Domain.ValueObjects;
using Infrastructure.Interfaces;
using Infrastructure.Security;
using Microsoft.Extensions.Logging;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserService:IUserService
    {
        private readonly ICrudRepository<User> _repository;
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly ILogger _logger;
        public UserService(ICrudRepository<User> repository, IUserRepository userRepository,IEmailService emailService,ILogger logger)
        {
            _repository = repository;
            _userRepository = userRepository;
            _emailService = emailService;
            _logger = logger;
        }
        public async Task<OperationResult> Register(string email,string password)
        {
            try
            {
                return await _repository.Add(new User()
                {
                    Email = new Email(email),
                    HashedPassword = PasswordService.HashPasswordWithSalt(password, (await _userRepository.GetLastId() + 1).ToString()),
                    RegDate = DateTime.Now,
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while registering a user.");
                return await OperationResult.CreateAsync(false, HttpStatusCode.InternalServerError);
            }
        }
        public async Task<OperationResult> Login(string email,string password)
        {
            try
            {
                var user = _userRepository.GetByEmail(email);
                if (user is not null)
                {
                    var salt = user.Id.ToString();
                    var hashedPassword = PasswordService.HashPasswordWithSalt(password, salt);
                    return await _userRepository.Login(email, hashedPassword);
                }
                return await OperationResult.CreateAsync(false, HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while signing in a user.");
                return await OperationResult.CreateAsync(false, HttpStatusCode.InternalServerError);
            }
        }
        public async Task<OperationResult> ResetPasswordRequest(string email)
        {
            try
            {
                var user = _userRepository.GetByEmail(email);
                if (user is not null)
                {
                    var resetCode = new Random().Next(1000, 10000);
                    var sendEmailResult = await _emailService.SendResetCodeEmail(email, resetCode);
                    if (sendEmailResult == new OperationResult(true, HttpStatusCode.Created))
                        return await _userRepository.ResetPasswordRequest(email, resetCode);
                    else
                        return await OperationResult.CreateAsync(false, sendEmailResult.ErrorCode);
                }
                return await OperationResult.CreateAsync(false, HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while applying for a reset password request");
                return await OperationResult.CreateAsync(false, HttpStatusCode.InternalServerError);
            }
        }
        public async Task<OperationResult> ResetPasswordVerify(string email,int resetCode)
        {
            try
            {
                var user = _userRepository.GetByEmail(email);
                if (user is not null)
                {
                    return await _userRepository.ResetPasswordRequestVerify(email, resetCode);
                }
                return await OperationResult.CreateAsync(false, HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while applying for a reset password verify");
                return await OperationResult.CreateAsync(false, HttpStatusCode.InternalServerError);
            }
        }
        public async Task<OperationResult> ResetPassword(string email, string newPassword)
        {
            try
            {
                var user = await _userRepository.GetByEmail(email);
                if (user is not null)
                {
                    return await _repository.Update(user.Id, new User()
                    {
                        Email = new Email(email),
                        HashedPassword = PasswordService.HashPasswordWithSalt(newPassword, user.Id.ToString()),
                        RegDate = user.RegDate,
                        UserStatus = user.UserStatus
                    });
                }
                return await OperationResult.CreateAsync(false, HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while reseting a password");
                return await OperationResult.CreateAsync(false, HttpStatusCode.InternalServerError);
            }
        }
    }
}


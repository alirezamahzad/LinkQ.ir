using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Shared;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        public EmailService(IConfiguration configuration,ILogger logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        public async Task<OperationResult> SendResetCodeEmail(string recipientEmail, int resetCode)
        {
            try
            {
                using (SmtpClient client = new SmtpClient(_configuration["SmtpSettings:Server"]))
                {
                    client.Port = int.Parse(_configuration["SmtpSettings:Port"]!);
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(_configuration["SmtpSettings:Username"], _configuration["SmtpSettings:Password"]);
                    client.EnableSsl = bool.Parse(_configuration["SmtpSettings:UseSsl"]!);

                    MailMessage mailMessage = new MailMessage
                    {
                        From = new MailAddress(_configuration["SmtpSettings:Username"]!),
                        Subject = "LinkQ.ir | Password Reset Code",
                        Body = CreateResetCodeEmailBody(resetCode.ToString()),
                        IsBodyHtml = true
                    };

                    mailMessage.To.Add(recipientEmail);

                    await client.SendMailAsync(mailMessage);
                    _logger.LogInformation($"Email Sent to :{recipientEmail},");
                    return await OperationResult.CreateAsync(true, HttpStatusCode.Created);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An Exception Occured while sending an email");
                return await OperationResult.CreateAsync(false, HttpStatusCode.InternalServerError);
            }
        }

        private string CreateResetCodeEmailBody(string resetCode)
        {
            try
            {
                StringBuilder bodyBuilder = new StringBuilder();
                bodyBuilder.AppendLine("<html>");
                bodyBuilder.AppendLine("<body>");
                bodyBuilder.AppendLine("<h2>کد تائید فراموشی رمز عبور</h2>");
                bodyBuilder.AppendLine("<p>کد فراموشی رمز :</p>");
                bodyBuilder.AppendLine($"<p style='font-size: 18px; font-weight: bold;'>{resetCode}</p>");
                bodyBuilder.AppendLine("<p>از این کد برای دسترسی تغییر رمزعبور حساب کاربری خود استفاده کنید</p>");
                bodyBuilder.AppendLine("<p>موفق باشید!</p>");
                bodyBuilder.AppendLine("</body>");
                bodyBuilder.AppendLine("</html>");

                return bodyBuilder.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while applying for a reset password request");
                return string.Empty;
            }
        }
    }
}

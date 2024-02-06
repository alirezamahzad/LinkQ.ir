using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IEmailService
    {
        Task<OperationResult> SendResetCodeEmail(string recipientEmail, int resetCode);
    }
}

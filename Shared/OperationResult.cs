using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class OperationResult
    {
        public bool IsSuccess { get; }
        public HttpStatusCode ErrorCode { get; }

        public OperationResult(bool isSuccess, HttpStatusCode errorCode)
        {
            IsSuccess = isSuccess;
            ErrorCode = errorCode;
        }

        // Asynchronous constructor
        public static async Task<OperationResult> CreateAsync(bool isSuccess, HttpStatusCode errorCode)
        {
            return await Task.FromResult(new OperationResult(isSuccess, errorCode));
        }
    }
}

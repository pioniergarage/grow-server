using Grow.Server.Model.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Grow.Server.Model.Middleware
{
    public class ExceptionLoggerMiddleware
    {
        private RequestDelegate Next { get; }
        public ILogger Logger { get; }

        public ExceptionLoggerMiddleware(RequestDelegate next, ILogger logger)
        {
            Next = next;
            Logger = logger;
        }
        
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await Next(context).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                HandleException(context, ex);
                throw;
            }
        }

        private void HandleException(HttpContext context, Exception exception)
        {
            string errorCode = CalculateErrorCode(context.TraceIdentifier);
            string message = string.Format(
                "Unhandled Exception: Error Code '{0}'  [{1}] \r\n {2}",
                errorCode, 
                context.TraceIdentifier,
                exception.Message
            );

            Logger.LogError(message, exception);
        }

        private static string CalculateErrorCode(string traceIdentifier)
        {
            const int ErrorCodeLength = 6;
            const string CodeValues = "BCDFGHJKLMNPQRSTVWXYZ";
            MD5 hasher = MD5.Create();
            StringBuilder sb = new StringBuilder(10);

            byte[] traceBytes = hasher.ComputeHash(Encoding.UTF8.GetBytes(traceIdentifier));
            int codeValuesLength = CodeValues.Length;

            for (int i = 0; i < ErrorCodeLength; i++)
            {
                sb.Append(CodeValues[traceBytes[i] % codeValuesLength]);
            }

            return sb.ToString();
        }
    }

    public static class ExceptionLoggerMiddlewareExtensions
    {
        public static IApplicationBuilder UseLoggingExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionLoggerMiddleware>();
        }
    }
}

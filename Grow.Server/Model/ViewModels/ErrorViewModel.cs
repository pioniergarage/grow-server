using System;

namespace Grow.Server.Model.ViewModels
{
    public class ErrorViewModel
    {
        public string ErrorMessage { get; set; }

        public string ErrorDetails { get; set; }

        public static ErrorViewModel FromException(Exception e)
        {
            return new ErrorViewModel
            {
                ErrorMessage = e.Message,
                ErrorDetails = e.StackTrace
            };
        }
    }
}

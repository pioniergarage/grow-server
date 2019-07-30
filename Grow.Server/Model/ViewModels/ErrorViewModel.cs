using System;

namespace Grow.Server.Model.ViewModels
{
    public class ErrorViewModel
    {
        public string ErrorMessage { get; set; } = "Unknown Server Error";

        public string ErrorDetails { get; set; } = "No further details are given";

        public static ErrorViewModel FromException(Exception e)
        {
            return new ErrorViewModel
            {
                ErrorMessage = e.Message,
                ErrorDetails = e.StackTrace
            };
        }

        public static ErrorViewModel FromStatusCode(string code)
        {
            var vm = new ErrorViewModel();

            switch (code)
            {
                case "400":
                    vm.ErrorMessage = "Bad Request";
                    vm.ErrorDetails = "Your request was invalid. Please go back and try again.";
                    break;
                case "403":
                    vm.ErrorMessage = "Forbidden";
                    vm.ErrorDetails = "You are not allowed to access this page";
                    break;
                case "404":
                    vm.ErrorMessage = "Not Found";
                    vm.ErrorDetails = "Seems like what you are looking for is no longer here.";
                    break;
                case "500":
                    vm.ErrorMessage = "Internal Error";
                    vm.ErrorDetails = "This should not have happened. Something went wrong and we'll try to fix it as soon as possible.";
                    break;
                default:
                    break;
            }

            return vm;
        }
    }
}

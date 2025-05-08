
namespace FinancialControl.Application.Core.Utilities
{
    public class Result
    {
        public bool IsSuccess { get; }
        public string ErrorMessage { get; }
        public bool IsFailure => !IsSuccess;

        protected Result(bool isSuccess, string errorMessage)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
        }

        public static Result Success() => new Result(true, string.Empty);
        public static Result Error(string message) => new Result(false, message);

    }
}

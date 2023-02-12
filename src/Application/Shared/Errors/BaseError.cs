using System.Diagnostics.CodeAnalysis;

namespace Application.Shared.Errors
{
    [ExcludeFromCodeCoverage]
    public class BaseError
    {
        public BaseError()
        {
            IsValid = true;
        }
        public BaseError(Error error)
        {
            Error = error;
            IsValid = false;
        }

        public bool IsValid { get; private set; }
        public Error Error { get; set; }
    }
}

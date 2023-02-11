namespace Application.Shared.Errors
{
    public class BaseError
    {
        public BaseError()
        {

        }
        public BaseError(Error? error)
        {
            Error = error;
        }

        public bool IsValid => Error != null;
        public Error? Error { get; set; } = null;
    }
}

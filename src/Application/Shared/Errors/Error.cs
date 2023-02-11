namespace Application.Shared.Errors
{
    public struct Error : IEquatable<Error>
    {
        public Error(string errorMessage, bool canRetry)
        {
            ErrorMessage = errorMessage;
            CanRetry = canRetry;
        }

        public string ErrorMessage { get; set; }
        public bool CanRetry { get; set; }

        public bool Equals(Error other)
        {
            if (other.ErrorMessage == ErrorMessage && other.CanRetry == CanRetry) return true;

            return false;
        }
    }
}

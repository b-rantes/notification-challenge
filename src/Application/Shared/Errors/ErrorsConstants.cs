namespace Application.Shared.Errors
{
    public static class ErrorsConstants
    {
        private const string FailFastValidationErrorMessage = "Error in fail fast validation";
        private const string InvalidBusinessRuleErrorMessage = "Error in business rule";
        private const string UnexpectedDependencyErrorMessage = "Error in some of the dependencies occurred unexpectedly";

        public static Error FailFastError { get; private set; } = new Error(FailFastValidationErrorMessage, canRetry: false);
        public static Error InvalidBusinessRuleError { get; private set; } = new Error(InvalidBusinessRuleErrorMessage, canRetry: false);
        public static Error UnexpectedDependencyError { get; private set; } = new Error(UnexpectedDependencyErrorMessage, canRetry: true);
    }
}

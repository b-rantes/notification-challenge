namespace Domain.Helpers
{
    public sealed class EntityValidationResult
    {
        internal List<string> _errors { get; set; } = new List<string>();
        internal void AddErrorMessage(string message) => _errors.Add(message);
        public bool IsValid => !_errors.Any();
        public List<string> GetErrors() => _errors;
    }
}

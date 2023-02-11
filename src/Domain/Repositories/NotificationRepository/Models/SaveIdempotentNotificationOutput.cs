namespace Domain.Repositories.NotificationRepository.Models
{
    public class SaveIdempotentNotificationOutput
    {
        public SaveIdempotentNotificationOutput()
        {
            ErrorMessage = string.Empty;
        }
        public SaveIdempotentNotificationOutput(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public bool NotificationSaved => string.IsNullOrEmpty(ErrorMessage);
        public string ErrorMessage { get; private set; }
    }
}

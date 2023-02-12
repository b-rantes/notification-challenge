using Application.Shared.Errors;

namespace Application.SyncServices.UpdateNotificationView.Models
{
    public class UpdateNotificationViewOutput : BaseError
    {
        public UpdateNotificationViewOutput() : base()
        {

        }

        public UpdateNotificationViewOutput(Error error) : base(error)
        {

        }

        public static UpdateNotificationViewOutput Success() => new UpdateNotificationViewOutput();
        public static UpdateNotificationViewOutput Fail(Error error) => new UpdateNotificationViewOutput(error);
    }
}

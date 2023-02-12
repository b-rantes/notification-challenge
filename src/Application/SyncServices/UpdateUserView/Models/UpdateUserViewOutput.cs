using Application.Shared.Errors;

namespace Application.SyncServices.UpdateUserView.Models
{
    public class UpdateUserViewOutput : BaseError
    {
        public UpdateUserViewOutput() : base()
        {

        }

        public UpdateUserViewOutput(Error error) : base(error)
        {

        }

        public static UpdateUserViewOutput Success() => new UpdateUserViewOutput();
        public static UpdateUserViewOutput Fail(Error error) => new UpdateUserViewOutput(error);
    }
}

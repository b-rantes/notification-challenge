using Application.Shared.Errors;

namespace Application.UseCases.UpsertUserControl.Models
{
    public class UpsertUserControlOutput : BaseError
    {
        public UpsertUserControlOutput() : base()
        {

        }

        public UpsertUserControlOutput(Error error) : base(error)
        {

        }

        public static UpsertUserControlOutput Success() => new UpsertUserControlOutput();
        public static UpsertUserControlOutput Fail(Error error) => new UpsertUserControlOutput(error);
    }
}

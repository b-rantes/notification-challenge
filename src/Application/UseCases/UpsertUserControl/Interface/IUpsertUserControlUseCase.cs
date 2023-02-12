using Application.UseCases.UpsertUserControl.Models;

namespace Application.UseCases.UpsertUserControl.Interface
{
    public interface IUpsertUserControlUseCase
    {
        public Task<UpsertUserControlOutput> UpsertUserSettingsAsync(UpsertUserControlInput input, CancellationToken cancellationToken);
    }
}

using Application.SyncServices.UpdateUserView.Models;

namespace Application.SyncServices.UpdateUserView.Interface
{
    public interface IUpdateUserViewService
    {
        public Task<UpdateUserViewOutput> UpdateUserViewAsync(UpdateUserViewInput input, CancellationToken cancellationToken);
    }
}

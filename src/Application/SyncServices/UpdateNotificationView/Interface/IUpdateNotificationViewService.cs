using Application.SyncServices.UpdateNotificationView.Models;

namespace Application.SyncServices.UpdateNotificationView.Interface
{
    public interface IUpdateNotificationViewService
    {
        public Task<UpdateNotificationViewOutput> UpdateNotificationViewAsync(UpdateNotificationViewInput input, CancellationToken cancellationToken);
    }
}

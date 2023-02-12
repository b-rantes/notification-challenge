using Domain.Repositories.UserRepository;
using Infrastructure.Cache.CachedUserView.Models;

namespace Infrastructure.Cache.Interfaces
{
    public interface ICachedUserViewRepository : IUserViewRepository
    {
        public Task UpdateUserViewAsync(UpdateCachedUserControlViewInput input, CancellationToken cancellationToken);
    }
}

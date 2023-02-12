using Domain.Repositories.UserRepository.Models;

namespace Domain.Repositories.UserRepository
{
    public interface IUserViewRepository
    {
        public Task<UserControlView> GetUserById(long id, CancellationToken cancellationToken);
    }
}

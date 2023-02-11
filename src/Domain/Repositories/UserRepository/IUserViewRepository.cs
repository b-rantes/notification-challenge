using Domain.DomainModels.Entities.UserAggregate;

namespace Domain.Repositories.UserRepository
{
    public interface IUserViewRepository
    {
        public Task<User> GetUserById(long id, CancellationToken cancellationToken);
    }
}

using Domain.Entities.UserAggregate;

namespace Domain.Repositories.UserAggregateRepository
{
    public interface IUserViewRepository
    {
        public Task<User> GetUserById(long id, CancellationToken cancellationToken);
    }
}

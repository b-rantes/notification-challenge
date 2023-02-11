using Domain.Entities.UserAggregate;

namespace Domain.Repositories.UserAggregateRepository
{
    public interface IUserCommandRepository
    {
        public Task UpsertUserAsync(User user, CancellationToken cancellationToken);
    }
}

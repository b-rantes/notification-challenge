using Domain.DomainModels.Entities.UserAggregate;

namespace Domain.Repositories.UserRepository
{
    public interface IUserCommandRepository
    {
        public Task UpsertUserAsync(User user, CancellationToken cancellationToken);
    }
}

using Domain.DomainModels.Entities.UserAggregate;
using Domain.Services.Models;

namespace Domain.Repositories.UserRepository
{
    public interface IUserCommandRepository
    {
        public Task UpsertUserAsync(UpsertUserInput upsertInput, CancellationToken cancellationToken);
    }
}

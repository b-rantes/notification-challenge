using Domain.Services.Models;

namespace Domain.Services.Interfaces
{
    public interface IUserManagerDomainService
    {
        public Task UpsertUserAsync(UpsertUserInput user, CancellationToken cancellationToken);
    }
}

using Domain.Events;
using Domain.Repositories.UserRepository;
using Domain.Services.Interfaces;
using Domain.Services.Models;

namespace Domain.Services
{
    public class UserManagerDomainService : IUserManagerDomainService
    {
        private readonly IDomainEventsProducer _domainEventsProducer;
        private readonly IUserCommandRepository _userCommandRepository;
        public UserManagerDomainService(IDomainEventsProducer domainEventsProducer, IUserCommandRepository userCommandRepository)
        {
            _domainEventsProducer = domainEventsProducer;
            _userCommandRepository = userCommandRepository;
        }

        public async Task UpsertUserAsync(UpsertUserInput user, CancellationToken cancellationToken)
        {
            try
            {
                await _userCommandRepository.UpsertUserAsync(user, cancellationToken);

                var userSettingsUpdated = new ProduceUserSettingsUpdatedInput
                {
                    Id = user.Id,
                    CanReceiveNotification = user.CanReceiveNotification,
                    LastOpenedNotificationDate = user.LastOpenedNotificationDate
                };

                await _domainEventsProducer.ProduceUserSettingsUpdatedEvent(userSettingsUpdated, cancellationToken);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

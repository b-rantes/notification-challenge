using Application.Shared.Errors;
using Application.UseCases.CreateUserNotification.Interface;
using Application.UseCases.CreateUserNotification.Mapper;
using Application.UseCases.CreateUserNotification.Models;
using Domain.DomainModels.Entities.NotificationAggregate;
using Domain.Repositories.UserRepository;
using Domain.Services.Interfaces;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.CreateUserNotification
{
    public class CreateUserNotificationUseCase : ICreateUserNotificationUseCase
    {
        private readonly ILogger<CreateUserNotificationUseCase> _logger;
        private readonly IValidator<CreateUserNotificationInput> _validator;
        private readonly IUserViewRepository _userViewRepository;
        private readonly INotificationManagerDomainService _notificationDomainService;

        public CreateUserNotificationUseCase(
            ILogger<CreateUserNotificationUseCase> logger,
            IValidator<CreateUserNotificationInput> validator,
            IUserViewRepository userViewRepository,
            INotificationManagerDomainService notificationDomainService)
        {
            _logger = logger;
            _validator = validator;
            _userViewRepository = userViewRepository;
            _notificationDomainService = notificationDomainService;
        }

        public async Task<CreateUserNotificationOutput> CreateUserNotificationAsync(CreateUserNotificationInput input, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(input, cancellationToken);

                if (!validationResult.IsValid)
                {
                    _logger.LogWarning("[{UseCase}] failed in fail fast validation. Errors: {errors}",
                        nameof(CreateUserNotificationUseCase), validationResult.Errors.Select(x => x.ErrorMessage).ToList());

                    return CreateUserNotificationOutput.Fail(ErrorsConstants.FailFastError);
                }

                var user = await _userViewRepository.GetUserById(input.UserId, cancellationToken);

                if (!user.CanReceiveNotification)
                {
                    _logger.LogInformation("[{UseCase}] not creating notification due to client: {client} turned off configuration",
                        nameof(CreateUserNotificationUseCase), user.Id);

                    return CreateUserNotificationOutput.Success();
                }

                var notification = input.MapInputToNotification();

                await _notificationDomainService.CreateUserNotificationAsync(user, notification, cancellationToken);
                
                return new CreateUserNotificationOutput();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[{UseCase}] unexpected error", nameof(CreateUserNotificationUseCase));
                throw;
            }
        }
    }
}

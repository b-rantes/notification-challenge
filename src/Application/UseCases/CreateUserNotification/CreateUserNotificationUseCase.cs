using Application.Shared.Errors;
using Application.UseCases.CreateUserNotification.Interface;
using Application.UseCases.CreateUserNotification.Mapper;
using Application.UseCases.CreateUserNotification.Models;
using Domain.Builders;
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
                _logger.LogInformation("[{UseCase}] started execution for id: {id}",
                    nameof(CreateUserNotificationUseCase), input.UserId);

                var validationResult = await _validator.ValidateAsync(input, cancellationToken);

                if (!validationResult.IsValid)
                {
                    _logger.LogWarning("[{UseCase}] failed in fail fast validation. Errors: {errors}",
                        nameof(CreateUserNotificationUseCase), validationResult.Errors.Select(x => x.ErrorMessage).ToList());

                    return CreateUserNotificationOutput.Fail(ErrorsConstants.FailFastError);
                }

                var userControlView = await _userViewRepository.GetUserById(input.UserId, cancellationToken);

                var user = UserBuilder.CreateUser()
                    .WithId(userControlView.Id)
                    .WithNotificationDeliveryControl(userControlView.LastOpenedNotificationDate)
                    .WithNotificationSettings(userControlView.CanReceiveNotification);

                if (!user.CanReceiveNotification)
                {
                    _logger.LogInformation("[{UseCase}] not creating notification due to client: {client} turned off configuration",
                        nameof(CreateUserNotificationUseCase), user.Id);

                    return CreateUserNotificationOutput.Success();
                }

                var notification = input.MapInputToNotification();

                await _notificationDomainService.CreateUserNotificationAsync(user, notification, cancellationToken);

                _logger.LogInformation("[{UseCase}] finished execution for id: {id} successfully",
                    nameof(CreateUserNotificationUseCase), input.UserId);
                return CreateUserNotificationOutput.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[{UseCase}] unexpected error", nameof(CreateUserNotificationUseCase));
                throw;
            }
        }
    }
}

using Application.Shared.Errors;
using Application.UseCases.FetchUserNotifications.Interface;
using Application.UseCases.FetchUserNotifications.Models;
using Domain.Builders;
using Domain.Repositories.UserRepository;
using Domain.Services.Interfaces;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.FetchUserNotifications
{
    public class FetchUserNotificationsUseCase : IFetchUserNotificationsUseCase
    {
        private readonly ILogger<FetchUserNotificationsUseCase> _logger;
        private readonly IValidator<FetchUserNotificationsInput> _validator;
        private readonly IUserViewRepository _userViewRepository;
        private readonly INotificationManagerDomainService _notificationDomainService;

        public FetchUserNotificationsUseCase(
            ILogger<FetchUserNotificationsUseCase> logger,
            IValidator<FetchUserNotificationsInput> validator,
            IUserViewRepository userViewRepository,
            INotificationManagerDomainService notificationDomainService)
        {
            _logger = logger;
            _validator = validator;
            _userViewRepository = userViewRepository;
            _notificationDomainService = notificationDomainService;
        }

        public async Task<FetchUserNotificationsOutput> FetchUserNotificationsAsync(FetchUserNotificationsInput input, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(input, cancellationToken);

                if (!validationResult.IsValid)
                {
                    _logger.LogWarning("[{UseCase}] failed in fail fast validation. Errors: {errors}",
                        nameof(FetchUserNotificationsUseCase), validationResult.Errors.Select(x => x.ErrorMessage).ToList());

                    return FetchUserNotificationsOutput.Fail(ErrorsConstants.FailFastError);
                }

                var userControlView = await _userViewRepository.GetUserById(input.UserId, cancellationToken);

                if (userControlView is null) return null;

                var user = UserBuilder.CreateUser()
                    .WithId(userControlView.Id)
                    .WithNotificationDeliveryControl(userControlView.LastOpenedNotificationDate)
                    .WithNotificationSettings(userControlView.CanReceiveNotification);

                await _notificationDomainService.FetchUserNotificationsAsync(input.UserId, cancellationToken);
                
                return FetchUserNotificationsOutput.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[{UseCase}] unexpected error", nameof(FetchUserNotificationsUseCase));
                throw;
            }
        }
    }
}

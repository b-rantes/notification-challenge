using Application.Shared.Errors;
using Application.UseCases.FetchUserNotifications.Interface;
using Application.UseCases.FetchUserNotifications.Models;
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
        private readonly INotificationManagerDomainService _notificationDomainService;

        public FetchUserNotificationsUseCase(
            ILogger<FetchUserNotificationsUseCase> logger,
            IValidator<FetchUserNotificationsInput> validator,
            INotificationManagerDomainService notificationDomainService)
        {
            _logger = logger;
            _validator = validator;
            _notificationDomainService = notificationDomainService;
        }

        public async Task<FetchUserNotificationsOutput> FetchUserNotificationsAsync(FetchUserNotificationsInput input, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("[{UseCase}] started execution for id: {id}",
                    nameof(FetchUserNotificationsUseCase), input.UserId); 
                
                var validationResult = await _validator.ValidateAsync(input, cancellationToken);

                if (!validationResult.IsValid)
                {
                    _logger.LogWarning("[{UseCase}] failed in fail fast validation. Errors: {errors}",
                        nameof(FetchUserNotificationsUseCase), validationResult.Errors.Select(x => x.ErrorMessage).ToList());

                    return FetchUserNotificationsOutput.Fail(ErrorsConstants.FailFastError);
                }

                var result = await _notificationDomainService.FetchUserNotificationsAsync(input.UserId, cancellationToken);


                _logger.LogInformation("[{UseCase}] finished execution for id: {id} successfully",
                    nameof(FetchUserNotificationsUseCase), input.UserId);

                return FetchUserNotificationsOutput.Success(result.Notifications, result.NewNotificationsCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[{UseCase}] unexpected error", nameof(FetchUserNotificationsUseCase));
                throw;
            }
        }
    }
}

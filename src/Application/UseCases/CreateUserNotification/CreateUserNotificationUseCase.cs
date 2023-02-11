using Application.Shared.Errors;
using Application.UseCases.CreateUserNotification.Interface;
using Application.UseCases.CreateUserNotification.Models;
using Domain.Repositories.UserAggregateRepository;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.CreateUserNotification
{
    public class CreateUserNotificationUseCase : ICreateUserNotificationUseCase
    {
        private readonly ILogger<CreateUserNotificationUseCase> _logger;
        private readonly IValidator<CreateUserNotificationInput> _validator;
        private readonly IUserViewRepository _userViewRepository;

        public CreateUserNotificationUseCase(
            ILogger<CreateUserNotificationUseCase> logger,
            IValidator<CreateUserNotificationInput> validator,
            IUserViewRepository userViewRepository)
        {
            _logger = logger;
            _validator = validator;
            _userViewRepository = userViewRepository;
        }
        /*
         * - Fail fast validation
         * - Busca redis com modelo de view dado do cliente.
         */

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

                if (user.IsNotificationOff)
                {
                    _logger.LogInformation("[{UseCase}] not creating notification due to client: {client} turned off configuration",
                        nameof(CreateUserNotificationUseCase), user.Id);

                    return CreateUserNotificationOutput.Success();
                }

                //Validate notification idempotence
                //Create notification
                //notification manager domain service -> User + Notification = Create notification for user

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

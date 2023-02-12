using Application.Shared.Errors;
using Application.SyncServices.UpdateNotificationView.Interface;
using Application.SyncServices.UpdateNotificationView.Mapper;
using Application.SyncServices.UpdateNotificationView.Models;
using FluentValidation;
using Infrastructure.Cache.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.SyncServices.UpdateNotificationView
{
    public class UpdateNotificationViewService : IUpdateNotificationViewService
    {
        private readonly ILogger<UpdateNotificationViewService> _logger;
        private readonly IValidator<UpdateNotificationViewInput> _validator;
        private readonly ICachedNotificationViewRepository _cachedNotificationViewRepository;

        public UpdateNotificationViewService(
            ILogger<UpdateNotificationViewService> logger,
            IValidator<UpdateNotificationViewInput> validator,
            ICachedNotificationViewRepository cachedNotificationViewRepository)
        {
            _logger = logger;
            _validator = validator;
            _cachedNotificationViewRepository = cachedNotificationViewRepository;
        }

        public async Task<UpdateNotificationViewOutput> UpdateNotificationViewAsync(UpdateNotificationViewInput input, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(input, cancellationToken);

                if (!validationResult.IsValid)
                {
                    _logger.LogWarning("[{UseCase}] failed in fail fast validation. Errors: {errors}",
                        nameof(UpdateNotificationViewService), validationResult.Errors.Select(x => x.ErrorMessage).ToList());

                    return UpdateNotificationViewOutput.Fail(ErrorsConstants.FailFastError);
                }

                var cachedNotificationView = input.MapInputToCachedNotificationView();

                await _cachedNotificationViewRepository.UpdateInCacheNotificationViewByUser(cachedNotificationView, cancellationToken);

                _logger.LogInformation("[{UseCase}] executed successfully for client: {userId}",
                    nameof(UpdateNotificationViewService), input.UserOwnerId);
                return UpdateNotificationViewOutput.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[{UseCase}] unexpected error", nameof(UpdateNotificationViewService));
                throw;
            }
        }
    }
}

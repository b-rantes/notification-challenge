using Application.Shared.Errors;
using Application.SyncServices.UpdateUserView.Interface;
using Application.SyncServices.UpdateUserView.Mapper;
using Application.SyncServices.UpdateUserView.Models;
using FluentValidation;
using Infrastructure.Cache.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.SyncServices.UpdateUserView
{
    public class UpdateUserViewService : IUpdateUserViewService
    {
        private readonly ILogger<UpdateUserViewService> _logger;
        private readonly IValidator<UpdateUserViewInput> _validator;
        private readonly ICachedUserViewRepository _cachedUserViewRepository;

        public UpdateUserViewService(
            ILogger<UpdateUserViewService> logger,
            IValidator<UpdateUserViewInput> validator,
            ICachedUserViewRepository cachedUserViewRepository)
        {
            _logger = logger;
            _validator = validator;
            _cachedUserViewRepository = cachedUserViewRepository;
        }

        public async Task<UpdateUserViewOutput> UpdateUserViewAsync(UpdateUserViewInput input, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("[{UseCase}] started execution for id: {id}",
                    nameof(UpdateUserViewService), input.UserId);

                var validationResult = await _validator.ValidateAsync(input, cancellationToken);

                if (!validationResult.IsValid)
                {
                    _logger.LogWarning("[{UseCase}] failed in fail fast validation. Errors: {errors}",
                        nameof(UpdateUserViewService), validationResult.Errors.Select(x => x.ErrorMessage).ToList());

                    return UpdateUserViewOutput.Fail(ErrorsConstants.FailFastError);
                }

                var cachedUserView = input.MapInputToCachedUserView();

                await _cachedUserViewRepository.UpdateUserViewAsync(cachedUserView, cancellationToken);

                return UpdateUserViewOutput.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[{UseCase}] unexpected error", nameof(UpdateUserViewService));
                throw;
            }
        }
    }
}

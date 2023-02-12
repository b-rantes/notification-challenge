using Application.Shared.Errors;
using Application.UseCases.UpsertUserControl.Interface;
using Application.UseCases.UpsertUserControl.Mapper;
using Application.UseCases.UpsertUserControl.Models;
using Domain.Repositories.UserRepository;
using Domain.Services.Interfaces;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.UpsertUserControl
{
    public class UpsertUserControlUseCase : IUpsertUserControlUseCase
    {
        private readonly ILogger<UpsertUserControlUseCase> _logger;
        private readonly IValidator<UpsertUserControlInput> _validator;
        private readonly IUserManagerDomainService _userDomainService;

        public UpsertUserControlUseCase(
            ILogger<UpsertUserControlUseCase> logger,
            IValidator<UpsertUserControlInput> validator,
            IUserManagerDomainService userDomainService)
        {
            _logger = logger;
            _validator = validator;
            _userDomainService = userDomainService;
        }

        public async Task<UpsertUserControlOutput> UpsertUserSettingsAsync(UpsertUserControlInput input, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("[{UseCase}] started execution for id: {id}",
                    nameof(UpsertUserControlUseCase), input.UserId);

                var validationResult = await _validator.ValidateAsync(input, cancellationToken);

                if (!validationResult.IsValid)
                {
                    _logger.LogWarning("[{UseCase}] failed in fail fast validation. Errors: {errors}",
                        nameof(UpsertUserControlUseCase), validationResult.Errors.Select(x => x.ErrorMessage).ToList());

                    return UpsertUserControlOutput.Fail(ErrorsConstants.FailFastError);
                }

                var upsertInput = input.MapInputToUpsertUserInput();

                await _userDomainService.UpsertUserAsync(upsertInput, cancellationToken);

                _logger.LogInformation("[{UseCase}] finished execution for id: {id} successfully",
                    nameof(UpsertUserControlUseCase), input.UserId);

                return UpsertUserControlOutput.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[{UseCase}] unexpected error", nameof(UpsertUserControlUseCase));
                throw;
            }
        }
    }
}

using Application.Shared.Errors;
using Application.UseCases.UpsertUserControl;
using Application.UseCases.UpsertUserControl.Interface;
using Application.UseCases.UpsertUserControl.Models;
using Application.UseCases.UpsertUserControl.Validators;
using AutoFixture;
using Bogus;
using Domain.Services.Interfaces;
using Domain.Services.Models;
using FluentAssertions;
using FluentValidation;
using Moq;
using Moq.AutoMock;

namespace UnitTests.Application.UpsertUserControl
{
    public class UpsertUserControlUseCaseTests
    {
        private readonly Faker _dataFaker;
        private readonly AutoMocker _autoMocker;
        private readonly IUpsertUserControlUseCase _useCase;
        private readonly Fixture _fixture;

        public UpsertUserControlUseCaseTests()
        {
            _autoMocker = new AutoMocker();
            _dataFaker = new Faker();
            _fixture = new Fixture();

            _autoMocker.Use<IValidator<UpsertUserControlInput>>(new UpsertUserControlInputValidator());
            _useCase = _autoMocker.CreateInstance<UpsertUserControlUseCase>();
        }

        [Fact(DisplayName = "Should return usecase fail fast validation error when invalid input data")]
        public async Task Should_Return_FailFastValidationError()
        {
            //Arrange
            var inputWithInvalidUserIdInput = GenerateValidInput();
            inputWithInvalidUserIdInput.UserId = _dataFaker.Random.Long(max: 0);

            //Act
            var result = await _useCase.UpsertUserSettingsAsync(inputWithInvalidUserIdInput, CancellationToken.None);

            //Assert
            result.IsValid.Should().BeFalse();
            result.Error.Equals(ErrorsConstants.FailFastError).Should().BeTrue();
        }


        [Fact(DisplayName = "Should return success and upsert successfully")]
        public async Task Should_Return_Success_And_User_Notifications()
        {
            //Arrange
            var input = GenerateValidInput();

            //Act
            var result = await _useCase.UpsertUserSettingsAsync(input, CancellationToken.None);

            //Assert
            result.IsValid.Should().BeTrue();
            _autoMocker.GetMock<IUserManagerDomainService>().Verify(x => x.UpsertUserAsync(It.IsAny<UpsertUserInput>(), It.IsAny<CancellationToken>()), Times.Once());
        }

        private UpsertUserControlInput GenerateValidInput() =>
            new UpsertUserControlInput
            {
                UserId = _dataFaker.Random.Long(min: 1),
                CanReceiveNotification = _dataFaker.Random.Bool(),
                LastOpenedNotificationDate = _dataFaker.Date.Recent()
            };
    }
}

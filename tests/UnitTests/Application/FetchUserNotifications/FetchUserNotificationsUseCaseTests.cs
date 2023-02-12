using Application.Shared.Errors;
using Application.UseCases.FetchUserNotifications;
using Application.UseCases.FetchUserNotifications.Interface;
using Application.UseCases.FetchUserNotifications.Models;
using Application.UseCases.FetchUserNotifications.Validators;
using AutoFixture;
using Bogus;
using Domain.DomainModels.Entities.NotificationAggregate;
using Domain.DomainModels.Entities.UserAggregate;
using Domain.Repositories.UserRepository;
using Domain.Repositories.UserRepository.Models;
using Domain.Services.Interfaces;
using Domain.Services.Models;
using FluentAssertions;
using FluentValidation;
using Moq;
using Moq.AutoMock;

namespace UnitTests.Application.FetchUserNotifications
{
    public class FetchUserNotificationsUseCaseTests
    {
        private readonly Faker _dataFaker;
        private readonly AutoMocker _autoMocker;
        private readonly IFetchUserNotificationsUseCase _useCase;
        private readonly Fixture _fixture;

        public FetchUserNotificationsUseCaseTests()
        {
            _autoMocker = new AutoMocker();
            _dataFaker = new Faker();
            _fixture = new Fixture();

            _autoMocker.Use<IValidator<FetchUserNotificationsInput>>(new FetchUserNotificationsInputValidator());
            _useCase = _autoMocker.CreateInstance<FetchUserNotificationsUseCase>();
        }

        [Fact(DisplayName = "Should return usecase fail fast validation error when invalid input data")]
        public async Task Should_Return_FailFastValidationError()
        {
            //Arrange
            var inputWithInvalidUserIdInput = GenerateValidInput();
            inputWithInvalidUserIdInput.UserId = _dataFaker.Random.Long(max: 0);

            //Act
            var result = await _useCase.FetchUserNotificationsAsync(inputWithInvalidUserIdInput, CancellationToken.None);

            //Assert
            result.IsValid.Should().BeFalse();
            result.Error.Equals(ErrorsConstants.FailFastError).Should().BeTrue();
        }


        [Fact(DisplayName = "Should return success and user notifications")]
        public async Task Should_Return_Success_And_User_Notifications()
        {
            //Arrange
            var input = GenerateValidInput();
            var mockedUser = GenerateUserControlView(input.UserId, _dataFaker.Date.Recent(), canReceiveNotification: true);

            _autoMocker.GetMock<INotificationManagerDomainService>().Setup(x => x.FetchUserNotificationsAsync(input.UserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(_fixture.Create<UserNotificationsOutput>());

            //Act
            var result = await _useCase.FetchUserNotificationsAsync(input, CancellationToken.None);

            //Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact(DisplayName = "Should throw when domain service throws")]
        public async Task Should_Throw_When_Domain_Throws()
        {
            //Arrange
            var input = GenerateValidInput();

            _autoMocker.GetMock<INotificationManagerDomainService>().Setup(x => x.FetchUserNotificationsAsync(input.UserId, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());

            //Act, Assert
            await Assert.ThrowsAsync<Exception>(async () => await _useCase.FetchUserNotificationsAsync(input, CancellationToken.None));
        }

        private FetchUserNotificationsInput GenerateValidInput() =>
            new FetchUserNotificationsInput
            {
                UserId = _dataFaker.Random.Long(min: 1)
            };

        private UserControlView GenerateUserControlView(long userId, DateTime lastOpenedNotificationDate, bool canReceiveNotification) =>
            new UserControlView
            {
                CanReceiveNotification = canReceiveNotification,
                LastOpenedNotificationDate = lastOpenedNotificationDate,
                Id = userId
            };
    }
}

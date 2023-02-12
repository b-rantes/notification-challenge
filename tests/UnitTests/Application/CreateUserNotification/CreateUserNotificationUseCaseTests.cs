using Application.Shared.Errors;
using Application.UseCases.CreateUserNotification;
using Application.UseCases.CreateUserNotification.Interface;
using Application.UseCases.CreateUserNotification.Models;
using Application.UseCases.CreateUserNotification.Validators;
using Bogus;
using Domain.Builders;
using Domain.DomainModels.Entities.NotificationAggregate;
using Domain.DomainModels.Entities.UserAggregate;
using Domain.Repositories.UserRepository;
using Domain.Repositories.UserRepository.Models;
using Domain.Services.Interfaces;
using FluentAssertions;
using FluentValidation;
using Moq;
using Moq.AutoMock;

namespace UnitTests.Application.CreateUserNotification
{
    public class CreateUserNotificationUseCaseTests
    {
        private readonly Faker _dataFaker;
        private readonly AutoMocker _autoMocker;
        private readonly ICreateUserNotificationUseCase _useCase;

        public CreateUserNotificationUseCaseTests()
        {
            _autoMocker = new AutoMocker();
            _dataFaker = new Faker();
            _autoMocker.Use<IValidator<CreateUserNotificationInput>>(new CreateUserNotificationInputValidator());
            _useCase = _autoMocker.CreateInstance<CreateUserNotificationUseCase>();
        }

        [Fact(DisplayName = "Should return usecase fail fast validation error when invalid input data")]
        public async Task Should_Return_FailFastValidationError()
        {
            //Arrange
            var inputWithInvalidUserIdInput = GenerateValidInput();
            inputWithInvalidUserIdInput.UserId = _dataFaker.Random.Long(max: 0);

            var inputWithInvalidNotificationId = GenerateValidInput();
            inputWithInvalidNotificationId.NotificationGuid = Guid.Empty;


            //Act
            var result1 = await _useCase.CreateUserNotificationAsync(inputWithInvalidUserIdInput, CancellationToken.None);
            var result2 = await _useCase.CreateUserNotificationAsync(inputWithInvalidNotificationId, CancellationToken.None);

            //Assert
            result1.IsValid.Should().BeFalse();
            result1.Error.Equals(ErrorsConstants.FailFastError).Should().BeTrue();

            result2.IsValid.Should().BeFalse();
            result2.Error.Equals(ErrorsConstants.FailFastError).Should().BeTrue();
        }

        [Fact(DisplayName = "Should return success when user cannot receive notification and not process")]
        public async Task Should_Return_Success_ButNotProcess()
        {
            //Arrange
            var input = GenerateValidInput();
            var mockedUser = GenerateUserControlView(input.UserId, _dataFaker.Date.Recent(), false);

            _autoMocker.GetMock<IUserViewRepository>().Setup(x => x.GetUserById(input.UserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockedUser);

            //Act
            var result = await _useCase.CreateUserNotificationAsync(input, CancellationToken.None);

            //Assert
            result.IsValid.Should().BeTrue();
            _autoMocker.GetMock<INotificationManagerDomainService>().Verify(x =>
                x.CreateUserNotificationAsync(
                    It.Is<User>(y => y.Id == input.UserId),
                    It.Is<Notification>(y => y.NotificationId == input.NotificationGuid),
                    It.IsAny<CancellationToken>()), Times.Never());
        }

        [Fact(DisplayName = "Should return success when user can receive notification and process")]
        public async Task Should_Return_Success_And_Process()
        {
            //Arrange
            var input = GenerateValidInput();
            var mockedUser = GenerateUserControlView(input.UserId, _dataFaker.Date.Recent(), canReceiveNotification: true);

            _autoMocker.GetMock<IUserViewRepository>().Setup(x => x.GetUserById(input.UserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockedUser);

            //Act
            var result = await _useCase.CreateUserNotificationAsync(input, CancellationToken.None);

            //Assert
            result.IsValid.Should().BeTrue();
            _autoMocker.GetMock<INotificationManagerDomainService>().Verify(x =>
                x.CreateUserNotificationAsync(
                    It.Is<User>(y => y.Id == input.UserId),
                    It.Is<Notification>(y => y.NotificationId == input.NotificationGuid),
                    It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact(DisplayName = "Should throw when repository throws")]
        public async Task Should_Throw_When_Repo_Throws()
        {
            //Arrange
            var input = GenerateValidInput();
            var mockedUser = UserBuilder
                .CreateUser()
                .WithId(input.UserId)
                .WithNotificationDeliveryControl(_dataFaker.Date.Recent())
                .WithNotificationSettings(true);

            _autoMocker.GetMock<IUserViewRepository>().Setup(x => x.GetUserById(input.UserId, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());

            //Act, Assert
            await Assert.ThrowsAsync<Exception>(async () => await _useCase.CreateUserNotificationAsync(input, CancellationToken.None));
        }

        [Fact(DisplayName = "Should throw when domain service throws")]
        public async Task Should_Throw_When_DomainServiceThrows()
        {
            //Arrange
            var input = GenerateValidInput();
            var mockedUser = GenerateUserControlView(input.UserId, _dataFaker.Date.Recent(), canReceiveNotification: true);

            _autoMocker.GetMock<IUserViewRepository>().Setup(x => x.GetUserById(input.UserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockedUser);

            _autoMocker.GetMock<INotificationManagerDomainService>().Setup(x =>
                x.CreateUserNotificationAsync(
                    It.Is<User>(y => y.Id == input.UserId),
                    It.Is<Notification>(y => y.NotificationId == input.NotificationGuid),
                    It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());

            //Act, Assert
            await Assert.ThrowsAsync<Exception>(async () => await _useCase.CreateUserNotificationAsync(input, CancellationToken.None));
        }

        private CreateUserNotificationInput GenerateValidInput() =>
            new CreateUserNotificationInput
            {
                NotificationGuid = Guid.NewGuid(),
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

using Bogus;
using Domain.Builders;
using Domain.DomainModels.Entities.NotificationAggregate;
using Domain.DomainModels.Entities.UserAggregate;
using Domain.Events;
using Domain.Exceptions;
using Domain.Repositories.NotificationRepository;
using Domain.Repositories.NotificationRepository.Models;
using Domain.Services;
using Domain.Services.Interfaces;
using Moq;
using Moq.AutoMock;

namespace UnitTests.Domain.Services
{
    public class NotificationManagerDomainTests
    {
        private readonly Faker _dataFaker;
        private readonly AutoMocker _autoMocker;
        private readonly INotificationManagerDomainService _domainService;

        public NotificationManagerDomainTests()
        {
            _autoMocker = new AutoMocker();
            _dataFaker = new Faker();
            _domainService = _autoMocker.CreateInstance<NotificationManagerDomainService>();
        }

        [Fact(DisplayName = "Invalid User should throw in Create User Notification and not process")]
        public async Task InvalidUser_ShouldThrow_And_Not_Process()
        {
            //Arrange
            var user = GenerateInvalidUser();

            //Act, Assert
            await Assert.ThrowsAsync<DomainException>(async () =>
                await _domainService.CreateUserNotificationAsync(user, It.IsAny<Notification>(), CancellationToken.None));
        }

        [Fact(DisplayName = "Valid User but not allowed to receive notification should not process")]
        public async Task ValidUser_WithNotificationsOff_ShouldNot_ReceiveNotification()
        {
            //Arrange
            var user = GenerateValidUser(canReceiveNotification: false);

            //Act, Assert
            await Assert.ThrowsAsync<DomainException>(async () =>
                await _domainService.CreateUserNotificationAsync(user, It.IsAny<Notification>(), CancellationToken.None));
        }

        [Fact(DisplayName = "Valid user but notification failed in idempotence should throw")]
        public async Task ValidUser_WithNotificationFailed_In_Idempotence_Should_Throw()
        {
            //Arrange
            var user = GenerateValidUser(canReceiveNotification: true);
            var idempotentOutput = new SaveIdempotentNotificationOutput(errorMessage: "Idempotent Failed");

            _autoMocker.GetMock<INotificationCommandRepository>().Setup(x =>
                x.SaveIdempotentNotificationAsync(It.Is<Notification>(y => y.UserOwnerId == user.Id), It.IsAny<CancellationToken>()))
                .ReturnsAsync(idempotentOutput);

            //Act, Assert
            await Assert.ThrowsAsync<DomainException>(async () =>
                           await _domainService.CreateUserNotificationAsync(user, It.IsAny<Notification>(), CancellationToken.None));
        }

        [Fact(DisplayName = "If any dependence throws than domain service should throw")]
        public async Task IfAny_Dependence_Throws_DomainService_Should_Throw()
        {
            //Arrange
            var user = GenerateValidUser(canReceiveNotification: true);

            _autoMocker.GetMock<INotificationCommandRepository>().Setup(x =>
                x.SaveIdempotentNotificationAsync(It.Is<Notification>(y => y.UserOwnerId == user.Id), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());

            //Act, Assert
            await Assert.ThrowsAsync<DomainException>(async () =>
                           await _domainService.CreateUserNotificationAsync(user, It.IsAny<Notification>(), CancellationToken.None));
        }

        [Fact(DisplayName = "If user is valid and notification correctly created and persisted, should produce domain event")]
        public async Task ShouldProduce_DomainEvent_WhenEverything_RunsFine()
        {
            //Arrange
            var user = GenerateValidUser(canReceiveNotification: true);
            var notification = GenerateNewNotification(user.Id);
            var validIdempotentOutput = new SaveIdempotentNotificationOutput();

            _autoMocker.GetMock<INotificationCommandRepository>().Setup(x =>
                x.SaveIdempotentNotificationAsync(It.Is<Notification>(y => y.UserOwnerId == user.Id), It.IsAny<CancellationToken>()))
                .ReturnsAsync(validIdempotentOutput);

            //Act
            await _domainService.CreateUserNotificationAsync(user, notification, CancellationToken.None);

            //Assert
            _autoMocker.GetMock<IDomainEventsProducer>().Verify(x =>
                x.ProduceNotificationCreatedEvent(It.Is<Notification>(y => y.NotificationId == notification.NotificationId),
                It.Is<User>(y => y.Id == user.Id),
                It.IsAny<CancellationToken>()), Times.Once());
        }

        private User GenerateInvalidUser() =>
            UserBuilder.CreateUser();

        private User GenerateValidUser(bool canReceiveNotification) =>
            UserBuilder.
                CreateUser()
                .WithId(_dataFaker.Random.Long(min: 1))
                .WithNotificationDeliveryControl(_dataFaker.Date.Recent())
                .WithNotificationSettings(canReceiveNotification);

        private Notification GenerateNewNotification(long userOwnerId) => new Notification(Guid.NewGuid(), userOwnerId);
    }
}

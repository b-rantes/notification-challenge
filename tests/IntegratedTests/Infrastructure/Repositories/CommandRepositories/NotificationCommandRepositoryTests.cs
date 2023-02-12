using Bogus;
using Domain.DomainModels.Entities.NotificationAggregate;
using Domain.Repositories.NotificationRepository;
using FluentAssertions;
using Infrastructure.Repositories.DTOs;
using IntegratedTests.Fixtures;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace IntegratedTests.Infrastructure.Repositories.CommandRepositories
{
    public class NotificationCommandRepositoryTests
    {
        private readonly Faker _dataFaker;

        private readonly WebApplicationFixture _webFixture;
        private readonly INotificationCommandRepository _notificationCommandRepository;
        private readonly IMongoCollection<NotificationCommandCollection> _notificationCommandCollection;

        public NotificationCommandRepositoryTests()
        {

            _dataFaker = new Faker();

            _webFixture = new WebApplicationFixture();
            _notificationCommandRepository = _webFixture.Services.GetRequiredService<INotificationCommandRepository>();
            _notificationCommandCollection = _webFixture.Services.GetRequiredService<IMongoDatabase>()
                .GetCollection<NotificationCommandCollection>(CollectionsConstants.NotificationCollectionName);
        }

        [Fact(DisplayName = "SaveIdempotent notification should create new notification correctly")]
        public async Task SaveIdempotentNotification_ShouldCreate_New_Notification_Correctly()
        {
            //Arrange
            var notification = GenerateNewlyCreatedNotification();
            var anyContent = new { Prop = "Value" };
            notification.SetNotificationContent(anyContent);
            notification.CreateNotification();

            //Act
            var output = await _notificationCommandRepository.SaveIdempotentNotificationAsync(notification, CancellationToken.None);

            var result = (await _notificationCommandCollection
                .FindAsync(x => x.NotificationId == notification.NotificationId.ToString()))
                .FirstOrDefault();

            //Assert
            result.NotificationId.Should().Be(notification.NotificationId.ToString());
            result.NotificationCreationDate.Should().BeCloseTo(notification.NotificationCreationDate!.Value, TimeSpan.FromSeconds(1));
            result.UserOwnerId.Should().Be(notification.UserOwnerId);
            output.NotificationSaved.Should().BeTrue();
        }

        [Fact(DisplayName = "SaveIdempotent notification should not create duplicated notification")]
        public async Task SaveIdempotentNotification_ShouldNotCreate_DuplicatedNotification()
        {
            //Arrange
            var notification = GenerateNewlyCreatedNotification();
            var anyContent = new { Prop = "Value" };
            notification.SetNotificationContent(anyContent);
            notification.CreateNotification();

            //Act
            var output = await _notificationCommandRepository.SaveIdempotentNotificationAsync(notification, CancellationToken.None);
            var idempotentBlockExpectedOutput = await _notificationCommandRepository.SaveIdempotentNotificationAsync(notification, CancellationToken.None);

            var result = (await _notificationCommandCollection
                .FindAsync(x => x.NotificationId == notification.NotificationId.ToString())).ToList();

            //Assert
            result.FirstOrDefault().NotificationId.Should().Be(notification.NotificationId.ToString());
            result.FirstOrDefault().NotificationCreationDate.Should().BeCloseTo(notification.NotificationCreationDate!.Value, TimeSpan.FromSeconds(1));
            result.FirstOrDefault().UserOwnerId.Should().Be(notification.UserOwnerId);
            output.NotificationSaved.Should().BeTrue();
            result.Count.Should().Be(1);
            idempotentBlockExpectedOutput.NotificationSaved.Should().BeFalse();
            idempotentBlockExpectedOutput.ErrorMessage.Should().NotBeNullOrEmpty();
        }

        private Notification GenerateNewlyCreatedNotification() =>
            new Notification(Guid.NewGuid(), _dataFaker.Random.Long(min: 1));
    }
}

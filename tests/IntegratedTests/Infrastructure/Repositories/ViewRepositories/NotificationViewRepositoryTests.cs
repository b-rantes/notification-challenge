using AutoFixture;
using Bogus;
using Domain.DomainModels.Entities.NotificationAggregate;
using Domain.Repositories.NotificationRepository;
using Domain.Repositories.NotificationRepository.Models;
using FluentAssertions;
using Infrastructure.Cache.CachedNotificationView.Models;
using Infrastructure.Cache.Interfaces;
using IntegratedTests.Fixtures;
using Microsoft.Extensions.DependencyInjection;

namespace IntegratedTests.Infrastructure.Repositories.ViewRepositories
{
    public class NotificationViewRepositoryTests
    {
        private readonly Faker _dataFaker;

        private readonly Fixture _fixture;

        private readonly WebApplicationFixture _webFixture;
        private readonly INotificationViewRepository _notificationViewRepository;
        private readonly INotificationCommandRepository _notificationCommandRepository;
        private readonly ICachedNotificationViewRepository _cachedNotificationViewRepository;

        public NotificationViewRepositoryTests()
        {
            _fixture = new Fixture();
            _dataFaker = new Faker();

            _webFixture = new WebApplicationFixture();
            _notificationViewRepository = _webFixture.Services.GetRequiredService<INotificationViewRepository>();
            _cachedNotificationViewRepository = _webFixture.Services.GetRequiredService<ICachedNotificationViewRepository>();
            _notificationCommandRepository = _webFixture.Services.GetRequiredService<INotificationCommandRepository>();
        }

        [Fact(DisplayName = "Get notifications by userId should return cached data information")]
        public async Task GetNotificationsByUserId_Should_Return_Notifications_From_Cache()
        {
            // Arrange
            var updateInCacheInput = GenerateNotificationsViewInCache();

            await _cachedNotificationViewRepository.UpdateInCacheNotificationViewByUser(updateInCacheInput, CancellationToken.None);

            // Act
            var result = await _notificationViewRepository.GetNotificationsByUserId(updateInCacheInput.UserId, CancellationToken.None);

            // Assert
            result.LastOpenedNotificationDate.Should().BeCloseTo(updateInCacheInput.LastOpenedNotificationDate!.Value, TimeSpan.FromSeconds(1));
            result.LastUpdate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(3));
            result.Notifications.Should().HaveCount(1);
            result.UserId.Should().Be(updateInCacheInput.UserId);
        }

        [Fact(DisplayName = "Get notifications by userId should return data not from cache")]
        public async Task GetNotificationsByUserId_Should_Return_Notifications_From_Repository()
        {
            // Arrange
            var notification = GenerateNotification();

            await _notificationCommandRepository.SaveIdempotentNotificationAsync(notification, CancellationToken.None);

            // Act
            var result = await _notificationViewRepository.GetNotificationsByUserId(notification.UserOwnerId, CancellationToken.None);
            var resultFromCache = await _cachedNotificationViewRepository.GetNotificationsByUserId(notification.UserOwnerId, CancellationToken.None);

            // Assert
            resultFromCache.Should().BeNull();
            result.LastUpdate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(3));
            result.Notifications.Should().HaveCount(1);
            result.UserId.Should().Be(notification.UserOwnerId);
        }

        private NotificationsViewByUserOutput GenerateNotificationsViewByUser() =>
            _fixture
                .Build<NotificationsViewByUserOutput>()
                .With(x => x.UserId, _dataFaker.Random.Long(min: 1))
                .Create();

        private UpdateInCacheNotificationInput GenerateNotificationsViewInCache() =>
            _fixture.Create<UpdateInCacheNotificationInput>();

        private Notification GenerateNotification()
        {
            var notification = new Notification(Guid.NewGuid(), _dataFaker.Random.Long(min: 1));
            notification.SetNotificationContent(new { Content = "content" });

            return notification;
        }
    }
}

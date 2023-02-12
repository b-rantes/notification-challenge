using Bogus;
using FluentAssertions;
using Infrastructure.Cache.CachedNotificationView.Models;
using Infrastructure.Cache.Interfaces;
using IntegratedTests.Fixtures;
using Microsoft.Extensions.DependencyInjection;

namespace IntegratedTests.Infrastructure.Cache.CachedNotificationView
{
    public class CachedNotificationViewRepositoryTests
    {
        private readonly Faker _dataFaker;

        private readonly WebApplicationFixture _webFixture;
        private readonly ICachedNotificationViewRepository _cachedNotificationViewRepository;
        public CachedNotificationViewRepositoryTests()
        {
            _dataFaker = new Faker();

            _webFixture = new WebApplicationFixture();
            _cachedNotificationViewRepository = _webFixture.Services.GetRequiredService<ICachedNotificationViewRepository>();
        }

        [Fact(DisplayName = "Insert new notification when none exists previously by user")]
        public async Task Insert_FirstNotification_ForUser()
        {
            //Arrange
            var input = new UpdateInCacheNotificationInput
            {
                NotificationId = Guid.NewGuid(),
                LastOpenedNotificationDate = _dataFaker.Date.Past(1),
                NotificationContent = new { Prop = "new notification content" },
                NotificationCreationDate = _dataFaker.Date.Recent(),
                UserId = _dataFaker.Random.Long(min: 1)
            };

            var existentNotificationsBeforeCache = await _cachedNotificationViewRepository.GetNotificationsByUserId(input.UserId, CancellationToken.None);
            existentNotificationsBeforeCache.Should().BeNull();

            //Act
            await _cachedNotificationViewRepository.UpdateInCacheNotificationViewByUser(input, CancellationToken.None);

            //Assert
            var notificationsAfterUpdateCache = await _cachedNotificationViewRepository.GetNotificationsByUserId(input.UserId, CancellationToken.None);

            notificationsAfterUpdateCache.Should().NotBeNull();
            notificationsAfterUpdateCache.Notifications.Should().HaveCount(1);
        }

        [Fact(DisplayName = "Insert notification in existent key should insert new notification")]
        public async Task Insert_Notifications_ShouldAdd_New_Notification()
        {
            //Arrange
            var input = GenerateNewCacheNotificationInput(_dataFaker.Random.Long(min: 1));
            await _cachedNotificationViewRepository.UpdateInCacheNotificationViewByUser(input, CancellationToken.None);

            //Act
            var newInput = GenerateNewCacheNotificationInput(input.UserId);
            await _cachedNotificationViewRepository.UpdateInCacheNotificationViewByUser(newInput, CancellationToken.None);

            //Assert
            var notificationsCached = await _cachedNotificationViewRepository.GetNotificationsByUserId(input.UserId, CancellationToken.None);

            notificationsCached.Should().NotBeNull();
            notificationsCached.Notifications.Should().HaveCount(2);
        }

        private UpdateInCacheNotificationInput GenerateNewCacheNotificationInput(long userId) =>
            new UpdateInCacheNotificationInput
            {
                NotificationId = Guid.NewGuid(),
                LastOpenedNotificationDate = _dataFaker.Date.Past(1),
                NotificationContent = new { Prop = "new notification content" },
                NotificationCreationDate = _dataFaker.Date.Recent(),
                UserId = userId
            };
    }
}

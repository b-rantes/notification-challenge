using Bogus;
using FluentAssertions;
using Infrastructure.Cache.CachedUserView.Models;
using Infrastructure.Cache.Interfaces;
using IntegratedTests.Fixtures;
using Microsoft.Extensions.DependencyInjection;

namespace IntegratedTests.Infrastructure.Cache.CachedUserView
{
    public class CachedUserViewRepositoryTests
    {
        private readonly Faker _dataFaker;

        private readonly WebApplicationFixture _webFixture;
        private readonly ICachedUserViewRepository _cachedUserViewRepository;

        public CachedUserViewRepositoryTests()
        {
            _dataFaker = new Faker();

            _webFixture = new WebApplicationFixture();
            _cachedUserViewRepository = _webFixture.Services.GetRequiredService<ICachedUserViewRepository>();
        }

        [Fact(DisplayName = "Upsert new user control view")]
        public async Task Upsert_UserControlView_InCache()
        {
            //Arrange
            var input = GenerateNewCacheNotificationInput(
                _dataFaker.Random.Long(min: 1), _dataFaker.Date.Recent().ToUniversalTime(), _dataFaker.Random.Bool());

            //Act
            await _cachedUserViewRepository.UpdateUserViewAsync(input, CancellationToken.None);

            //Assert
            var userControlView = await _cachedUserViewRepository.GetUserById(input.Id, CancellationToken.None);
            userControlView.Id.Should().Be(input.Id);
            userControlView.CanReceiveNotification.Should().Be(input.CanReceiveNotification);
            userControlView.LastOpenedNotificationDate.Should().Be(input.LastOpenedNotificationDate);
        }

        [Fact(DisplayName = "IF CanReceiveNotification flag sent is null, do not update it")]
        public async Task DoNot_Update_CanReceiveNotification_IfInputNull()
        {
            //Arrange
            var input = GenerateNewCacheNotificationInput(
                _dataFaker.Random.Long(min: 1), _dataFaker.Date.Recent().ToUniversalTime(), _dataFaker.Random.Bool());
            await _cachedUserViewRepository.UpdateUserViewAsync(input, CancellationToken.None);

            var oldCachedUserView = await _cachedUserViewRepository.GetUserById(input.Id, CancellationToken.None);

            input.CanReceiveNotification = null;

            //Act
            await _cachedUserViewRepository.UpdateUserViewAsync(input, CancellationToken.None);

            //Assert
            var userControlView = await _cachedUserViewRepository.GetUserById(input.Id, CancellationToken.None);
            userControlView.Id.Should().Be(input.Id);
            userControlView.CanReceiveNotification.Should().Be(oldCachedUserView.CanReceiveNotification);
            userControlView.LastOpenedNotificationDate.Should().Be(input.LastOpenedNotificationDate);
        }

        [Fact(DisplayName = "IF LastOpenedNotificationDate sent is null, do not update it")]
        public async Task DoNot_Update_LastOpenedNotificationDate_IfInputNull()
        {
            //Arrange
            var input = GenerateNewCacheNotificationInput(
                _dataFaker.Random.Long(min: 1), _dataFaker.Date.Recent().ToUniversalTime(), _dataFaker.Random.Bool());
            await _cachedUserViewRepository.UpdateUserViewAsync(input, CancellationToken.None);

            var oldCachedUserView = await _cachedUserViewRepository.GetUserById(input.Id, CancellationToken.None);

            input.LastOpenedNotificationDate = null;

            //Act
            await _cachedUserViewRepository.UpdateUserViewAsync(input, CancellationToken.None);

            //Assert
            var userControlView = await _cachedUserViewRepository.GetUserById(input.Id, CancellationToken.None);
            userControlView.Id.Should().Be(input.Id);
            userControlView.CanReceiveNotification.Should().Be(input.CanReceiveNotification);
            userControlView.LastOpenedNotificationDate.Should().Be(oldCachedUserView.LastOpenedNotificationDate);
        }

        private UpdateCachedUserControlViewInput GenerateNewCacheNotificationInput(
            long userId, DateTime lastOpenedNotificationDate, bool canReceiveNotification) =>
            new UpdateCachedUserControlViewInput
            {
                Id = userId,
                CanReceiveNotification = canReceiveNotification,
                LastOpenedNotificationDate = lastOpenedNotificationDate,
            };
    }
}

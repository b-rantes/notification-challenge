using Bogus;
using Domain.Repositories.UserRepository;
using FluentAssertions;
using Infrastructure.Repositories.DTOs;
using IntegratedTests.Fixtures;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace IntegratedTests.Infrastructure.Repositories.ViewRepositories
{
    public class UserViewRepositoryTests
    {
        private readonly Faker _dataFaker;

        private readonly WebApplicationFixture _webFixture;
        private readonly IUserViewRepository _userViewRepository;
        private readonly IMongoCollection<UserViewCollection> _userViewCollection;

        public UserViewRepositoryTests()
        {
            _dataFaker = new Faker();

            _webFixture = new WebApplicationFixture();
            _userViewRepository = _webFixture.Services.GetRequiredService<IUserViewRepository>();
            _userViewCollection = _webFixture.Services.GetRequiredService<IMongoDatabase>()
                .GetCollection<UserViewCollection>(CollectionsConstants.UserCollectionName);
        }

        [Fact(DisplayName = "Existing item in repository should return correctly")]
        public async Task ExistingItem_Should_Return_Correctly()
        {
            //Arrange
            var userCollection = GenerateValidCollectionUser(_dataFaker.Random.Long(min: 1), _dataFaker.Random.Bool(), _dataFaker.Date.Recent().ToUniversalTime());
            _userViewCollection.InsertOne(userCollection);

            //Act
            var user = await _userViewRepository.GetUserById(userCollection.Id, CancellationToken.None);

            //Assert
            user.Should().NotBeNull();
            user.CanReceiveNotification.Should().Be(userCollection.CanReceiveNotification);
            user.LastOpenedNotificationDate.Should().BeCloseTo(userCollection.LastOpenedNotificationDate, TimeSpan.FromSeconds(1));
        }

        [Fact(DisplayName = "Inexistent item in repository should return null")]
        public async Task Inexistent_Item_ShouldNot_Return()
        {
            //Arrange, Act
            var user = await _userViewRepository.GetUserById(_dataFaker.Random.Long(min: 1), CancellationToken.None);

            //Assert
            user.Should().BeNull();
        }

        private UserViewCollection GenerateValidCollectionUser(long id, bool canReceiveNotification, DateTime lastOpenedNotificationDate) =>
            new UserViewCollection
            {
                Id = id,
                CanReceiveNotification = canReceiveNotification,
                LastOpenedNotificationDate = lastOpenedNotificationDate,
            };
    }
}

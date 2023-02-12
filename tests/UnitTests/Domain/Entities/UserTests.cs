using Bogus;
using Domain.Builders;
using Domain.DomainModels.Entities.UserAggregate;
using FluentAssertions;

namespace UnitTests.Domain.Entities
{
    public class UserTests
    {
        private readonly Faker _dataFaker;

        public UserTests()
        {
            _dataFaker = new Faker();
        }

        [Fact(DisplayName = "User entity should update LastOpenedNotificationDate when notification is open")]
        public void Should_UpdateLastOpenedNotificationDate_When_Notification_IsOpened()
        {
            //Arrange
            var user = GenerateValidUser(isNotificationOn: true);
            var lastOpenedNotificationDate = user.LastOpenedNotificationDate;

            //Act
            user.OpenNotification();

            //Assert
            user.LastOpenedNotificationDate.Should().BeAfter(lastOpenedNotificationDate!.Value);
        }

        private User GenerateValidUser(bool isNotificationOn) =>
            UserBuilder.CreateUser()
            .WithId(_dataFaker.Random.Long(min: 1))
            .WithNotificationDeliveryControl(lastOpenedNotificationDate: _dataFaker.Date.Recent())
            .WithNotificationSettings(isNotificationOn);
    }
}

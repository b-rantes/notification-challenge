using Bogus;
using Domain.Builders;
using Domain.Entities.UserAggregate;
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

        [Fact(DisplayName = "User entity TurnOnNotification method should turn notifications on")]
        public void TurnOnNotification_Should_Turn_Notifications_On()
        {
            //Arrange
            var user = GenerateValidUser(isNotificationOn: false);

            //Act
            user.TurnOnNotifications();

            //Assert
            user.IsNotificationOn.Should().BeTrue();
        }

        [Fact(DisplayName = "User entity TurnOffNotification method should turn notifications off")]
        public void TurnOffNotification_Should_Turn_Notifications_Off()
        {
            //Arrange
            var user = GenerateValidUser(isNotificationOn: true);

            //Act
            user.TurnOffNotifications();

            //Assert
            user.IsNotificationOn.Should().BeFalse();
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
            user.LastOpenedNotificationDate.Should().BeAfter(lastOpenedNotificationDate);
        }

        private User GenerateValidUser(bool isNotificationOn) =>
            UserBuilder.CreateUser()
            .WithId(_dataFaker.Random.Long(min: 1))
            .WithNotificationDeliveryControl(lastOpenedNotificationDate: _dataFaker.Date.Recent())
            .WithNotificationSettings(isNotificationOn);
    }
}

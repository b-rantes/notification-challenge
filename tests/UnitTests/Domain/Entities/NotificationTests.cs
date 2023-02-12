using Bogus;
using Domain.DomainModels.Entities.NotificationAggregate;
using Domain.DomainModels.Enuns;
using Domain.Exceptions;
using FluentAssertions;

namespace UnitTests.Domain.Entities
{
    public class NotificationTests
    {
        private readonly Faker _dataFaker;

        public NotificationTests()
        {
            _dataFaker = new Faker();
        }

        [Fact(DisplayName = "New public constructor should create notificationCreationDate, notificationContent and notificationState defaults")]
        public void PublicConstructor_ShouldCreate_Entity_WithNotificationCreationDate_And_Content_Null()
        {
            //Arrange, Act
            var notification = new Notification(Guid.NewGuid(), _dataFaker.Random.Long(min: 1));

            //Assert
            notification.NotificationCreationDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
            notification.NotificationContent.Should().BeNull();
        }

        [Fact(DisplayName = "Existent notification should create with parameters in constructor correctly")]
        public void Existent_Notification_Created_With_Parameters_In_Constructor()
        {
            //Arrange, Act
            var anyContentObject = new { Prop = "Value" };
            var userOwnerId = _dataFaker.Random.Long(min: 1);
            var notificationGuid = Guid.NewGuid();
            var notificationCreationDate = _dataFaker.Date.Recent();
            var notification = new Notification(notificationGuid, userOwnerId, notificationCreationDate, anyContentObject);

            //Assert
            notification.NotificationCreationDate.Should().Be(notificationCreationDate);
            notification.NotificationContent.Should().Be(anyContentObject);
            notification.NotificationId.Should().Be(notificationGuid);
            notification.UserOwnerId.Should().Be(userOwnerId);
        }

        [Fact(DisplayName = "SetNotificationContent should set NotificationContent correctly")]
        public void SetNotificationContent_ShouldSet_NotificationContent_Correctly()
        {
            //Arrange
            var notification = GenerateNewlyCreatedNotification();
            var anyContent = new { Prop = "value" };

            //Act
            notification.SetNotificationContent(anyContent);

            //Assert
            notification.NotificationContent.Should().Be(anyContent);
        }

        private Notification GenerateNewlyCreatedNotification() =>
            new Notification(Guid.NewGuid(), _dataFaker.Random.Long(min: 1));

        private Notification GenerateExistentNotification(Guid notificationId, long userOwnerId, DateTime notificationCreationDate, object? content = null)
            => new Notification(notificationId, userOwnerId, notificationCreationDate, content);
    }
}

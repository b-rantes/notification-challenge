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
            notification.NotificationCreationDate.Should().BeNull();
            notification.NotificationContent.Should().BeNull();
            notification.NotificationState.Should().Be(NotificationState.NotCreated);
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
            notification.NotificationState.Should().Be(NotificationState.Created);
        }

        [Fact(DisplayName = "CreateNotification should define NotificationCreationDate as DateTimeUtcNow and change state")]
        public void CreateNotification_ShouldDefine_NotificationCreationDate_AndChange_State()
        {
            //Arrange
            var notification = GenerateNewlyCreatedNotification();

            //Act
            notification.CreateNotification();

            //Assert
            notification.NotificationCreationDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
            notification.NotificationState.Should().Be(NotificationState.Created);
        }

        [Fact(DisplayName = "CreateNotification in already created notification should throw")]
        public void CreateNotification_ShouldThrow_WhenNotificationAlreadyCreated()
        {
            //Arrange
            var existentNotification = GenerateExistentNotification(Guid.NewGuid(), _dataFaker.Random.Long(min: 1), _dataFaker.Date.Recent());
            var newlyCreatedNotification = GenerateNewlyCreatedNotification();

            //Act, Assert
            Assert.Throws<DomainException>(() => existentNotification.CreateNotification());

            newlyCreatedNotification.CreateNotification();
            Assert.Throws<DomainException>(() => newlyCreatedNotification.CreateNotification());
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

        [Fact(DisplayName = "Notification without CreateNotificationMethod should be NotCreated state")]
        public void Notification_WithoutCreateNotificationMethod_ShouldBe_NotCreated_State()
        {
            //Arrange
            var notification = GenerateNewlyCreatedNotification();

            //Act, Assert
            notification.NotificationState.Should().Be(NotificationState.NotCreated);
        }

        private Notification GenerateNewlyCreatedNotification() =>
            new Notification(Guid.NewGuid(), _dataFaker.Random.Long(min: 1));

        private Notification GenerateExistentNotification(Guid notificationId, long userOwnerId, DateTime notificationCreationDate, object? content = null)
            => new Notification(notificationId, userOwnerId, notificationCreationDate, content);
    }
}

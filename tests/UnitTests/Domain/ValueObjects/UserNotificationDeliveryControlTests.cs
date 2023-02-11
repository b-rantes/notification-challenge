using Domain.ValueObjects;
using FluentAssertions;

namespace UnitTests.Domain.ValueObjects
{
    public class UserNotificationDeliveryControlTests
    {
        [Fact(DisplayName = "UserNotificationDeliveryControl value object should throw when not properly initialized")]
        public void Should_Throw_When_Validated_And_NotProperly_Initialized()
        {
            //Arrange
            var utcNowReference = DateTime.UtcNow;
            DateTime defaultDateTime = default;

            //Act, Assert
            Assert.Throws<ArgumentException>(() => new UserNotificationDeliveryControl(defaultDateTime).Validate());
            Assert.Throws<ArgumentException>(() => new UserNotificationDeliveryControl().Validate());
        }

        [Fact(DisplayName = "UserNotificationDeliveryControl value object should be created successfully when properly initialized")]
        public void Should_BeCreated_Successfully()
        {
            //Arrange
            var validLastOpenedNotificationDate = DateTime.UtcNow;

            //Act
            var validValueObject = new UserNotificationDeliveryControl(validLastOpenedNotificationDate);

            //Assert
            validValueObject.Should().NotBeNull();
            validValueObject.LastOpenedNotificationDate.Should().Be(validLastOpenedNotificationDate);
        }
    }
}

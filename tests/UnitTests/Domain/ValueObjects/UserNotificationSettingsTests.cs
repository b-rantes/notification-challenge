using Domain.DomainModels.ValueObjects;
using FluentAssertions;

namespace UnitTests.Domain.ValueObjects
{
    public class UserNotificationSettingsTests
    {
        [Fact(DisplayName = "Default UserNotificationSettings shoud always be ON")]
        public void Default_Parameterless_Constructor_Should_Set_UserNotification_ToOn()
        {
            //Arrange, Act
            var defaultValueObject = new UserNotificationSettings();

            //Assert
            defaultValueObject.CanReceiveNotification.Should().BeTrue();
        }

        [Theory(DisplayName = "UserNotificationSettings should initialize with defined parameter")]
        [InlineData(true)]
        [InlineData(false)]
        public void Should_Initialize_WithDefined_Parameter(bool parameter)
        {
            //Arrange, Act
            var valueObject = new UserNotificationSettings(parameter);

            //Assert
            valueObject.CanReceiveNotification.Should().Be(parameter);
        }
    }
}

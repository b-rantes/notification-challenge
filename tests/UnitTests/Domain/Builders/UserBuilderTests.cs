using Bogus;
using Domain.Builders;
using FluentAssertions;

namespace UnitTests.Domain.Builders
{
    public class UserBuilderTests
    {
        private readonly Faker _dataFaker;

        public UserBuilderTests()
        {
            _dataFaker = new Faker();
        }

        [Fact(DisplayName = "Builder should generate an invalid user when invalid Id")]
        public void Should_Generate_InvalidUser_When_InvalidId()
        {
            //Arrange
            var userWithEmptyId = UserBuilder
                .CreateUser()
                .WithNotificationDeliveryControl(lastOpenedNotificationDate: _dataFaker.Date.Recent())
                .WithNotificationSettings(isNotificationOn: _dataFaker.Random.Bool());

            var userWithZeroId = UserBuilder
                .CreateUser()
                .WithNotificationDeliveryControl(lastOpenedNotificationDate: _dataFaker.Date.Recent())
                .WithNotificationSettings(isNotificationOn: _dataFaker.Random.Bool())
                .WithId(0);

            var userWithInvalidId = UserBuilder
                .CreateUser()
                .WithNotificationDeliveryControl(lastOpenedNotificationDate: _dataFaker.Date.Recent())
                .WithNotificationSettings(isNotificationOn: _dataFaker.Random.Bool())
                .WithId(_dataFaker.Random.Long(max: 0));

            //Act
            var emptyIdValidationResult = userWithEmptyId.Validate();
            var zeroIdValidationResult = userWithZeroId.Validate();
            var invalidIdValidationResult = userWithInvalidId.Validate();

            //Assert
            emptyIdValidationResult.IsValid.Should().BeFalse();
            zeroIdValidationResult.IsValid.Should().BeFalse();
            invalidIdValidationResult.IsValid.Should().BeFalse();
        }

        [Fact(DisplayName = "Builder should generate an invalid user when invalid LastOpenedNotificationDate")]
        public void Should_Generate_InvalidUser_When_InvalidLastOpenedNotificationDate()
        {
            //Arrange
            var userWithDefaultLastOpenedValidationDate = UserBuilder
                .CreateUser()
                .WithId(_dataFaker.Random.Long(min: 1))
                .WithNotificationSettings(isNotificationOn: _dataFaker.Random.Bool())
                .WithNotificationDeliveryControl(lastOpenedNotificationDate: DateTime.MinValue);

            //Act
            var validationResult = userWithDefaultLastOpenedValidationDate.Validate();

            //Assert
            validationResult.IsValid.Should().BeFalse();
        }

        [Fact(DisplayName = "Builder should build correctly and generate valid user when all valid parameters")]
        public void Should_Generate_ValidUser_When_ValidParameters()
        {
            //Arrange
            var validId = _dataFaker.Random.Long(min: 1);
            var validIsNotificationOn = _dataFaker.Random.Bool();
            var validLastOpenedNotificationDate = _dataFaker.Date.Recent();
            var user = UserBuilder
                .CreateUser()
                .WithId(validId)
                .WithNotificationSettings(isNotificationOn: validIsNotificationOn)
                .WithNotificationDeliveryControl(lastOpenedNotificationDate: validLastOpenedNotificationDate);

            //Act
            var validationResult = user.Validate();

            //Assert
            validationResult.IsValid.Should().BeTrue();
            user.Id.Should().Be(validId);
            user.CanReceiveNotification.Should().Be(validIsNotificationOn);
            user.LastOpenedNotificationDate.Should().Be(validLastOpenedNotificationDate);
        }
    }
}

using Application.Shared.Errors;
using Application.SyncServices.UpdateNotificationView;
using Application.SyncServices.UpdateNotificationView.Interface;
using Application.SyncServices.UpdateNotificationView.Models;
using Application.SyncServices.UpdateNotificationView.Validators;
using Bogus;
using FluentAssertions;
using FluentValidation;
using Moq.AutoMock;

namespace UnitTests.SyncServices.UpdateNotificationView
{
    public class UpdateNotificationViewTests
    {
        private readonly Faker _dataFaker;
        private readonly AutoMocker _autoMocker;
        private readonly IUpdateNotificationViewService _notificationViewService;

        public UpdateNotificationViewTests()
        {
            _autoMocker = new AutoMocker();
            _dataFaker = new Faker();
            _autoMocker.Use<IValidator<UpdateNotificationViewInput>>(new UpdateNotificationViewInputValidator());
            _notificationViewService = _autoMocker.CreateInstance<UpdateNotificationViewService>();
        }

        [Fact(DisplayName = "Fail in Fail fast validation if invalid input")]
        public async Task FailFastValidation_Should_Return_Error()
        {
            //Arrange
            var invalidNotificationCreationDate = GenerateValidInput();
            invalidNotificationCreationDate.NotificationCreationDate = default;

            var invalidNotificationId = GenerateValidInput();
            invalidNotificationId.NotificationId = default;

            var invalidUserOwnerId = GenerateValidInput();
            invalidUserOwnerId.UserOwnerId = _dataFaker.Random.Long(max: 0);

            //Act
            var result1 = await _notificationViewService.UpdateNotificationViewAsync(invalidNotificationCreationDate, CancellationToken.None);
            var result2 = await _notificationViewService.UpdateNotificationViewAsync(invalidNotificationId, CancellationToken.None);
            var result3 = await _notificationViewService.UpdateNotificationViewAsync(invalidUserOwnerId, CancellationToken.None);

            //Assert
            result1.IsValid.Should().BeFalse();
            result1.Error.Equals(ErrorsConstants.FailFastError).Should().BeTrue();
            result2.IsValid.Should().BeFalse();
            result2.Error.Equals(ErrorsConstants.FailFastError).Should().BeTrue();
            result3.IsValid.Should().BeFalse();
            result3.Error.Equals(ErrorsConstants.FailFastError).Should().BeTrue();
        }

        [Fact(DisplayName = "Valid input should update correctly cache")]
        public async Task ValidInput_ShouldUpdate_Correctly()
        {
            //Arrange
            var input = GenerateValidInput();

            //Act
            var result = await _notificationViewService.UpdateNotificationViewAsync(input, CancellationToken.None);

            //Assert
            result.IsValid.Should().BeTrue();
        }

        private UpdateNotificationViewInput GenerateValidInput() =>
            new()
            {
                LastOpenedNotificationDate = _dataFaker.Date.Past(1),
                NotificationContent = new { Prop = "any content" },
                NotificationCreationDate = _dataFaker.Date.Recent(),
                NotificationId = Guid.NewGuid(),
                UserOwnerId = _dataFaker.Random.Long(min: 1),
            };
    }
}

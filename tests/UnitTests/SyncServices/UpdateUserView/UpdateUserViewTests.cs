using Application.Shared.Errors;
using Application.SyncServices.UpdateUserView;
using Application.SyncServices.UpdateUserView.Interface;
using Application.SyncServices.UpdateUserView.Models;
using Application.SyncServices.UpdateUserView.Validators;
using Bogus;
using FluentAssertions;
using FluentValidation;
using Moq.AutoMock;

namespace UnitTests.SyncServices.UpdateUserView
{
    public class UpdateUserViewTests
    {
        private readonly Faker _dataFaker;
        private readonly AutoMocker _autoMocker;
        private readonly IUpdateUserViewService _userViewService;

        public UpdateUserViewTests()
        {
            _autoMocker = new AutoMocker();
            _dataFaker = new Faker();
            _autoMocker.Use<IValidator<UpdateUserViewInput>>(new UpdateUserViewInputValidator());
            _userViewService = _autoMocker.CreateInstance<UpdateUserViewService>();
        }

        [Fact(DisplayName = "Fail in Fail fast validation if invalid input")]
        public async Task FailFastValidation_Should_Return_Error()
        {
            //Arrange
            var invalidUserId = GenerateValidInput();
            invalidUserId.UserId = _dataFaker.Random.Long(max: 0);

            //Act
            var result = await _userViewService.UpdateUserViewAsync(invalidUserId, CancellationToken.None);

            //Assert
            result.IsValid.Should().BeFalse();
            result.Error.Equals(ErrorsConstants.FailFastError).Should().BeTrue();
        }

        [Fact(DisplayName = "Valid input should update correctly cache")]
        public async Task ValidInput_ShouldUpdate_Correctly()
        {
            //Arrange
            var input = GenerateValidInput();

            //Act
            var result = await _userViewService.UpdateUserViewAsync(input, CancellationToken.None);

            //Assert
            result.IsValid.Should().BeTrue();
        }

        private UpdateUserViewInput GenerateValidInput() =>
            new()
            {
                LastOpenedNotificationDate = _dataFaker.Date.Past(1),
                CanReceiveNotification = _dataFaker.Random.Bool(),
                UserId = _dataFaker.Random.Long(min: 1)
            };
    }
}

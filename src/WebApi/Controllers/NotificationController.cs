using Application.UseCases.CreateUserNotification.Interface;
using Application.UseCases.FetchUserNotifications.Interface;
using Application.UseCases.FetchUserNotifications.Models;
using Application.UseCases.UpsertUserControl.Interface;
using Application.UseCases.UpsertUserControl.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("notifications")]
    public class NotificationController : ControllerBase
    {
        private readonly IFetchUserNotificationsUseCase _fetchUserNotificationsUseCase;
        private readonly IUpsertUserControlUseCase _upsertUserControlUseCase;

        public NotificationController(
            IFetchUserNotificationsUseCase fetchUserNotificationsUseCase,
            IUpsertUserControlUseCase upsertUserControlUseCase)
        {
            _upsertUserControlUseCase = upsertUserControlUseCase;
            _fetchUserNotificationsUseCase = fetchUserNotificationsUseCase;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserNotifications(long userId, CancellationToken cancellationToken)
        {
            try
            {
                var input = new FetchUserNotificationsInput { UserId = userId };

                var notifications = await _fetchUserNotificationsUseCase.FetchUserNotificationsAsync(input, cancellationToken);

                if (notifications is null) return BadRequest("User not found");

                return Ok(notifications);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost("user/{userId}/turn-on")]
        public async Task<IActionResult> TurnOnUserNotification(long userId, CancellationToken cancellationToken)
        {
            try
            {
                var input = new UpsertUserControlInput
                {
                    UserId = userId,
                    CanReceiveNotification = true
                };

                var result = await _upsertUserControlUseCase.UpsertUserSettingsAsync(input, cancellationToken);

                if (result.IsValid) return Ok();

                return BadRequest(result.Error.ErrorMessage);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost("user/{userId}/turn-off")]
        public async Task<IActionResult> TurnOffUserNotification(long userId, CancellationToken cancellationToken)
        {
            try
            {
                var input = new UpsertUserControlInput
                {
                    UserId = userId,
                    CanReceiveNotification = false
                };

                var result = await _upsertUserControlUseCase.UpsertUserSettingsAsync(input, cancellationToken);

                if (result.IsValid) return Ok();

                return BadRequest(result.Error.ErrorMessage);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
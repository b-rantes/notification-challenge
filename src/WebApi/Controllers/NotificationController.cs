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

        /// <summary>
        /// Get user notifications. After opened, a previously "newNotification" notification
        /// will be fetched, but will be marked as false.
        /// Will return 204 if user is not found
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{userId}")]
        [ProducesResponseType(typeof(FetchUserNotificationsOutput), 200)]
        public async Task<IActionResult> GetUserNotifications(long userId, CancellationToken cancellationToken)
        {
            try
            {
                var input = new FetchUserNotificationsInput { UserId = userId };

                var notifications = await _fetchUserNotificationsUseCase.FetchUserNotificationsAsync(input, cancellationToken);

                if (notifications is null) return NoContent();

                return Ok(notifications);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Route used to opt-in user notifications
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("user/{userId}/turn-on")]
        [ProducesResponseType(typeof(UpsertUserControlOutput), 200)]
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

        /// <summary>
        /// Route used to opt-out user notifications
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("user/{userId}/turn-off")]
        [ProducesResponseType(typeof(UpsertUserControlOutput), 200)]
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
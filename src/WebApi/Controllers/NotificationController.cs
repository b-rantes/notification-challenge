using Application.UseCases.CreateUserNotification.Interface;
using Application.UseCases.FetchUserNotifications.Interface;
using Application.UseCases.FetchUserNotifications.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("notifications")]
    public class NotificationController : ControllerBase
    {
        private readonly ICreateUserNotificationUseCase _createUserNotificationUseCase;
        private readonly IFetchUserNotificationsUseCase _fetchUserNotificationsUseCase;

        public NotificationController(
            ICreateUserNotificationUseCase createUserNotificationUseCase,
            IFetchUserNotificationsUseCase fetchUserNotificationsUseCase)
        {
            _createUserNotificationUseCase = createUserNotificationUseCase;
            _fetchUserNotificationsUseCase = fetchUserNotificationsUseCase;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> CreateUserNotification(long id, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _createUserNotificationUseCase.CreateUserNotificationAsync(
                    new() { UserId = id, NotificationGuid = Guid.NewGuid() },
                    cancellationToken);

                return Ok();

            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserNotifications(long id, CancellationToken cancellationToken)
        {
            try
            {
                var input = new FetchUserNotificationsInput { UserId = id };

                var notifications = await _fetchUserNotificationsUseCase.FetchUserNotificationsAsync(input, cancellationToken);

                if (notifications is null) return BadRequest("User not found");

                return Ok(notifications);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
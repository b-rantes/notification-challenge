using Application.UseCases.CreateUserNotification.Interface;
using Microsoft.AspNetCore.Mvc;
using WebApi.Controllers.AdminControllerModels;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("admin")]
    public class AdminController : ControllerBase
    {
        private readonly ICreateUserNotificationUseCase _createUserNotificationUseCase;
        public AdminController(ICreateUserNotificationUseCase createUserNotificationUseCase)
        {
            _createUserNotificationUseCase = createUserNotificationUseCase;
        }

        [HttpPost("notification-create")]
        public async Task<IActionResult> CreateUserNotification([FromBody] CreateNotificationRequest request,
            CancellationToken cancellationToken)
        {
            try
            {
                var output = await _createUserNotificationUseCase.CreateUserNotificationAsync(
                    new() { UserId = request.UserId, NotificationGuid = request.NotificationId, NotificationContent = request.NotificationContent },
                    cancellationToken);

                if (output.IsValid) return Ok(output);
                else return BadRequest(output);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

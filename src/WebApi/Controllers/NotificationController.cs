using Application.Shared.Errors;
using Application.UseCases.CreateUserNotification.Interface;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Net;
using System.Net.Sockets;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly ILogger<NotificationController> _logger;
        private readonly ICreateUserNotificationUseCase _createUserNotificationUseCase;
        private readonly IMongoClient _mongo;

        public NotificationController(ICreateUserNotificationUseCase createUserNotificationUseCase)
        {
            _createUserNotificationUseCase = createUserNotificationUseCase;
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
    }
}
using Application.UseCases.CreateUserNotification.Interface;
using Application.UseCases.CreateUserNotification.Models;
using Application.UseCases.UpsertUserControl.Interface;
using Application.UseCases.UpsertUserControl.Models;
using Microsoft.AspNetCore.Mvc;
using WebApi.Consumers.Base;
using WebApi.Controllers.AdminControllerModels;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("admin")]
    public class AdminController : ControllerBase
    {
        private readonly ICreateUserNotificationUseCase _createUserNotificationUseCase;

        private readonly IUpsertUserControlUseCase _upsertUserControlUseCase;
        private readonly IBaseProducer _producer;

        public AdminController(ICreateUserNotificationUseCase createUserNotificationUseCase,
            IBaseProducer producer,
            IUpsertUserControlUseCase upsertUserControlUseCase)
        {
            _createUserNotificationUseCase = createUserNotificationUseCase;
            _upsertUserControlUseCase = upsertUserControlUseCase;
            _producer = producer;
        }

        /// <summary>
        /// Criar uma notificação para um cliente específico.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("notification-create")]
        [ProducesResponseType(typeof(CreateUserNotificationOutput), 200)]
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

        /// <summary>
        /// Create a batch operation from initialId to lastId, creating every user and its Id
        /// with the defined boolean state. Only for purposes and initial load test.
        /// After using the route, wait until the execution is over
        /// Mantenha o "LastOpenedNotificationDate como null (não preencha o campo)
        /// </summary>
        /// <param name="initialId"></param>
        /// <param name="lastId"></param>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("user-initial-load/startId/{initialId}/{lastId}")]
        public async Task<IActionResult> CreateUserInBatch(int initialId, int lastId, [FromBody] UpsertUserControlInput request,
            CancellationToken cancellationToken)
        {
            try
            {
                if (lastId - initialId > 100) return BadRequest("Escolha intervalores menores");
                for (int i = initialId; i < lastId; i++)
                {
                    request.UserId = i;
                    await _upsertUserControlUseCase.UpsertUserSettingsAsync(request, cancellationToken);
                }

                return Ok();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

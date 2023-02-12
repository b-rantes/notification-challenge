using Application.UseCases.CreateUserNotification.Interface;
using Application.UseCases.CreateUserNotification.Models;
using Application.UseCases.UpsertUserControl.Interface;
using Application.UseCases.UpsertUserControl.Models;
using Infrastructure.EventProducer;
using Microsoft.AspNetCore.Mvc;
using WebApi.Consumers.Base;
using WebApi.Consumers.CommandConsumers.CreateUserNotificationCommand.Model;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("admin")]
    public class AdminController : ControllerBase
    {
        private readonly IUpsertUserControlUseCase _upsertUserControlUseCase;
        private readonly IBaseProducer _producer;

        public AdminController(
            IBaseProducer producer,
            IUpsertUserControlUseCase upsertUserControlUseCase)
        {
            _upsertUserControlUseCase = upsertUserControlUseCase;
            _producer = producer;
        }

        /// <summary>
        /// Criar uma notificação para um cliente específico.
        /// Caso deseje agendá-la, basta enviar um DateTime válido para a prop
        /// ScheduledNotificationUtcDate no body, em UTC.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("notification-create")]
        [ProducesResponseType(typeof(CreateUserNotificationOutput), 200)]
        public async Task<IActionResult> CreateUserNotification([FromBody] CreateUserNotificationCommandMessage request,
            CancellationToken cancellationToken)
        {
            try
            {
                await _producer.ProduceAsync(KafkaTopicsConstants.CreateUserNotificationTopic, request.UserId.ToString(), request, cancellationToken);

                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
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

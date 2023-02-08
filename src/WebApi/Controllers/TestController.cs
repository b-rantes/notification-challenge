using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;
        private readonly IMongoClient _mongo;
        public TestController(ILogger<TestController> logger, IMongoClient mongo)
        {
            _logger = logger;
            _mongo = mongo;
        }

        [HttpGet("test")]
        public async Task<IActionResult> Get()
        {
            var queryResult = _mongo.GetDatabase("MDB_MELI_NOTIFICATION").GetCollection<BsonDocument>("test").Find(_ => true);

            var result = queryResult.FirstOrDefault().ToJson();

            return Ok(result);
        }
    }
}
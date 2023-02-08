using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Net;
using System.Net.Sockets;

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
            // Host info
            var name = Dns.GetHostName(); // get container id
            var ip = Dns.GetHostEntry(name).AddressList.FirstOrDefault(x => x.AddressFamily == AddressFamily.InterNetwork);
            Console.WriteLine($"Host Name: {Environment.MachineName} \t {name}\t {ip} Executing Test Route");
            var queryResult = _mongo.GetDatabase("MDB_MELI_NOTIFICATION").GetCollection<BsonDocument>("test").Find(_ => true);

            var result = queryResult.FirstOrDefault().ToJson();

            return Ok(result);
        }
    }
}
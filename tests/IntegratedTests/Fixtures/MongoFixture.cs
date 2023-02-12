using Infrastructure.Repositories.DTOs;
using MongoDB.Driver;

namespace IntegratedTests.Fixtures
{
    public class MongoFixture
    {
        private readonly IMongoCollection<UserCommandCollection> _userViewCollection;
        public MongoFixture(IMongoDatabase db)
        {

        }


    }
}

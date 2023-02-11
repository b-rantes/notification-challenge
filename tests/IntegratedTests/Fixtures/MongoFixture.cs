using Infrastructure.Repositories.DTOs;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegratedTests.Fixtures
{
    public class MongoFixture
    {
        private readonly IMongoCollection<UserViewCollection> _userViewCollection;
        public MongoFixture(IMongoDatabase db)
        {

        }


    }
}

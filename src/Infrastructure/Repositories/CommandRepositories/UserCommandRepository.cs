using Domain.Repositories.UserRepository;
using Domain.Services.Models;
using Infrastructure.Repositories.DTOs;
using MongoDB.Driver;

namespace Infrastructure.Repositories.CommandRepositories
{
    public class UserCommandRepository : IUserCommandRepository
    {
        private readonly IMongoCollection<UserCommandCollection> _collection;

        public UserCommandRepository(IMongoDatabase db)
        {
            _collection = db.GetCollection<UserCommandCollection>(CollectionsConstants.UserCollectionName);
        }

        public async Task UpsertUserAsync(UpsertUserInput upsertInput, CancellationToken cancellationToken)
        {
            try
            {
                var filter = Builders<UserCommandCollection>.Filter
                    .Eq(x => x.Id, upsertInput.Id);

                var updateOperationList = new List<UpdateDefinition<UserCommandCollection>>();

                var upsertId = Builders<UserCommandCollection>.Update
                    .SetOnInsert(x => x.Id, upsertInput.Id);

                updateOperationList.Add(upsertId);

                var updateOperationForCanReceiveNotification = Builders<UserCommandCollection>.Update
                    .Set(x => x.CanReceiveNotification, upsertInput.CanReceiveNotification);

                if (upsertInput.CanReceiveNotification is not null)
                    updateOperationList.Add(updateOperationForCanReceiveNotification);

                var updateOperationForLastOpenedNotificationDate = Builders<UserCommandCollection>.Update
                    .Set(x => x.LastOpenedNotificationDate, upsertInput.LastOpenedNotificationDate);

                if (upsertInput.LastOpenedNotificationDate is not null)
                    updateOperationList.Add(updateOperationForLastOpenedNotificationDate);

                var updateOperation = Builders<UserCommandCollection>.Update
                    .Combine(updateOperationList);

                await _collection.UpdateOneAsync(filter, updateOperation, new UpdateOptions { IsUpsert = true }, cancellationToken);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

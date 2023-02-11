using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.DTOs
{
    public class UserViewCollection
    {
        [BsonId]
        public ObjectId Identification { get; set; }

        public long Id { get; set; }

        public DateTime LastOpenedNotificationDate { get; set; }

        public bool CanReceiveNotification { get; set; }
    }
}

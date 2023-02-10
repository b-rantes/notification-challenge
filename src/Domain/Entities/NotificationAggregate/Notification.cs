using Domain.Entities.Abstractions;

namespace Domain.Entities.NotificationAggregate
{
    public class Notification : IAggregateRoot
    {
        public long Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}

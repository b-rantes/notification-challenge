using Domain.Entities.Abstractions;

namespace Domain.Entities.NotificationAggregate
{
    public sealed class Notification : IEntity, IAggregateRoot
    {
        public Notification()
        {

        }
        public long Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Guid CorrelationId { get; set; }
        public DateTime NotificationDate { get; set; }
    }
}

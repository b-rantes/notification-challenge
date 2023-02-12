namespace WebApi.Consumers.Base
{
    public interface IBaseProducer
    {
        public Task ProduceAsync(string topic, string key, object message, CancellationToken cancellationToken);
    }
}

namespace APIProject.Infraestructure.Messaging
{
    public interface IEventPublisher
    {
        Task PublishAsync(string eventType, string payload, CancellationToken cancellationToken = default);
    }
}

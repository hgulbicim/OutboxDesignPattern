namespace OutboxDesignPattern.Jobs.OutboxPublisher
{
    public interface IOutboxPublisherService
    {
        Task ProcessOutboxEventsAsync();
    }
}

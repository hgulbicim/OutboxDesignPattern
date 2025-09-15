using OutboxDesignPattern.Data.Entity;
using OutboxDesignPattern.Events;

namespace OutboxDesignPattern.Services.Outbox
{
    public interface IOutboxService
    {
        Task SaveEventAsync<T>(T eventData, string eventType) where T : IOutboxEvent;
        Task<List<OutboxEventEntity>> GetUnprocessedEventsAsync(int batchSize = 10);
        Task MarkAsProcessedAsync(Guid eventId);
        Task MarkAsFailedAsync(Guid eventId, string errorMessage);
        Task IncrementRetryCountAsync(Guid eventId);
    }
}

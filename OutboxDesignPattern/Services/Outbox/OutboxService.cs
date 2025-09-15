using Microsoft.EntityFrameworkCore;
using OutboxDesignPattern.Data;
using OutboxDesignPattern.Data.Entity;
using OutboxDesignPattern.Events;
using System.Text.Json;

namespace OutboxDesignPattern.Services.Outbox
{
    public class OutboxService : IOutboxService
    {
        private readonly AppDbContext _context;

        public OutboxService(AppDbContext context)
        {
            _context = context;
        }

        public async Task SaveEventAsync<T>(T eventData, string eventType) where T : IOutboxEvent
        {
            var outboxEvent = new OutboxEventEntity
            {
                EventType = eventType,
                EventData = JsonSerializer.Serialize(eventData),
                CreatedAt = DateTime.UtcNow
            };

            _context.OutboxEvents.Add(outboxEvent);
            await _context.SaveChangesAsync();
        }

        public async Task<List<OutboxEventEntity>> GetUnprocessedEventsAsync(int batchSize = 10)
        {
            return await _context.OutboxEvents
                .Where(e => !e.IsProcessed && e.RetryCount < 3)
                .OrderBy(e => e.CreatedAt)
                .Take(batchSize)
                .ToListAsync();
        }

        public async Task MarkAsProcessedAsync(Guid eventId)
        {
            var outboxEvent = await _context.OutboxEvents.FindAsync(eventId);
            if (outboxEvent != null)
            {
                outboxEvent.IsProcessed = true;
                outboxEvent.ProcessedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task MarkAsFailedAsync(Guid eventId, string errorMessage)
        {
            var outboxEvent = await _context.OutboxEvents.FindAsync(eventId);
            if (outboxEvent != null)
            {
                outboxEvent.ErrorMessage = errorMessage;
                await _context.SaveChangesAsync();
            }
        }

        public async Task IncrementRetryCountAsync(Guid eventId)
        {
            var outboxEvent = await _context.OutboxEvents.FindAsync(eventId);
            if (outboxEvent != null)
            {
                outboxEvent.RetryCount++;
                await _context.SaveChangesAsync();
            }
        }
    }
}
using MassTransit;
using Microsoft.Extensions.Logging;
using Outbox.Events.V1;
using OutboxDesignPattern.Data.Entity;
using OutboxDesignPattern.Events;
using OutboxDesignPattern.Services.Outbox;
using System.Text.Json;

namespace OutboxDesignPattern.Jobs.OutboxPublisher
{
    public class OutboxPublisherService : IOutboxPublisherService
    {
        private readonly IOutboxService _outboxService;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<OutboxPublisherService> _logger;
        private static readonly Type[] _eventTypes;

        static OutboxPublisherService()
        {
            var assembly = typeof(UserCreated).Assembly;
            _eventTypes = assembly.GetTypes()
                .Where(t => typeof(IOutboxEvent).IsAssignableFrom(t) && t.IsClass)
                .ToArray();
        }

        public OutboxPublisherService(IOutboxService outboxService, IPublishEndpoint publishEndpoint, ILogger<OutboxPublisherService> logger)
        {
            _outboxService = outboxService;
            _publishEndpoint = publishEndpoint;
            _logger = logger;
        }

        public async Task ProcessOutboxEventsAsync()
        {
            while (true)
            {
                var events = await _outboxService.GetUnprocessedEventsAsync(10);
                if (events == null || !events.Any())
                    break;

                foreach (var evt in events)
                    await ProcessEventAsync(evt);
            }
        }

        private async Task ProcessEventAsync(OutboxEventEntity evt)
        {
            try
            {
                await PublishEventAsync(evt);
                await _outboxService.MarkAsProcessedAsync(evt.Id);
                _logger.LogInformation($"Event {evt.Id} successfully published.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error publishing event {evt.Id}.");
                await _outboxService.IncrementRetryCountAsync(evt.Id);
                await _outboxService.MarkAsFailedAsync(evt.Id, ex.Message);
            }
        }

        private async Task PublishEventAsync(OutboxEventEntity evt)
        {
            var type = _eventTypes.FirstOrDefault(t => t.Name == evt.EventType);
            if (type == null)
                throw new InvalidOperationException($"Unknown event type: {evt.EventType}");

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var deserialized = JsonSerializer.Deserialize(evt.EventData, type, options);

            await _publishEndpoint.Publish((dynamic)deserialized);
        }
    }
}

using Microsoft.Extensions.Logging;
using OutboxDesignPattern.Jobs.OutboxPublisher;
using Quartz;

namespace OutboxDesignPattern.Jobs
{
    [DisallowConcurrentExecution]
    public class OutboxProcessorJob : IJob
    {
        private readonly IOutboxPublisherService _outboxPublisherService;
        private readonly ILogger<OutboxProcessorJob> _logger;

        public OutboxProcessorJob(IOutboxPublisherService outboxPublisherService, ILogger<OutboxProcessorJob> logger)
        {
            _outboxPublisherService = outboxPublisherService;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("Outbox processor job started.");

            try
            {
                await _outboxPublisherService.ProcessOutboxEventsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during outbox processor job execution.");
            }

            _logger.LogInformation("Outbox processor job completed.");
        }
    }
}
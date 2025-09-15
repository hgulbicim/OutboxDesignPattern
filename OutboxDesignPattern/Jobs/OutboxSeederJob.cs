using Microsoft.Extensions.Logging;
using Outbox.Events.V1;
using OutboxDesignPattern.Services.Outbox;
using Quartz;

namespace OutboxDesignPattern.Jobs
{
    [DisallowConcurrentExecution]
    public class OutboxSeederJob : IJob
    {
        private readonly IOutboxService _outboxService;
        private readonly ILogger<OutboxSeederJob> _logger;

        public OutboxSeederJob(
            IOutboxService outboxService,
            ILogger<OutboxSeederJob> logger)
        {
            _outboxService = outboxService;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            for (int i = 0; i < 100; i++)
            {
                var userName = $"RandomUser_{Guid.NewGuid():N}";
                var userCreatedEvent = new UserCreated(userName);
                await _outboxService.SaveEventAsync(userCreatedEvent, nameof(UserCreated));
            }
        }
    }
}

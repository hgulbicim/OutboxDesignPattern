using OutboxDesignPattern.Events;

namespace Outbox.Events.V1
{
    public record UserCreated(string Name) : IOutboxEvent;
}

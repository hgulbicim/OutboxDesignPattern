using System.ComponentModel.DataAnnotations;

namespace OutboxDesignPattern.Data.Entity
{
    public class OutboxEventEntity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string EventType { get; set; }
        public string EventData { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsProcessed { get; set; } = false;
        public DateTime? ProcessedAt { get; set; }
        public int RetryCount { get; set; } = 0;
        public string? ErrorMessage { get; set; }
    }
}

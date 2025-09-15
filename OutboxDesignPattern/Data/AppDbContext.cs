using Microsoft.EntityFrameworkCore;
using OutboxDesignPattern.Data.Entity;

namespace OutboxDesignPattern.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<UserEntity> Users => Set<UserEntity>();
        public DbSet<OutboxEventEntity> OutboxEvents => Set<OutboxEventEntity>();
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OutboxEventEntity>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.EventType).IsRequired().HasMaxLength(200);
                entity.Property(e => e.EventData).IsRequired();
                entity.HasIndex(e => new { e.IsProcessed, e.CreatedAt });
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}

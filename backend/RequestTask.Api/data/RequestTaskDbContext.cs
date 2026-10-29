
using Microsoft.EntityFrameworkCore;
using RequestTask.Api.Models;

namespace RequestTask.Api.Data;

public class RequestTaskDbContext : DbContext
{
    public RequestTaskDbContext(
        DbContextOptions<RequestTaskDbContext> options)
        : base(options)
    {
    }

    public DbSet<Ticket> Tickets => Set<Ticket>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(t => t.Id);

            entity.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(t => t.Description)
                .IsRequired();

            entity.Property(t => t.Type)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(t => t.Priority)
                .HasMaxLength(50);

            entity.Property(t => t.Status)
                .HasMaxLength(50);
        });
    }
}

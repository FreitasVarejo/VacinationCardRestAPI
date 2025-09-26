using Microsoft.EntityFrameworkCore;
using VaccinationCard.Application.Abstractions;
using VaccinationCard.Domain.Entities;

namespace VaccinationCard.Infrastructure.Persistence;

public class AppDbContext : DbContext, IAppDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Person> Persons => Set<Person>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>(b =>
        {
            b.ToTable("Persons");
            b.HasKey(p => p.PersonId);
            b.Property(p => p.Name).IsRequired().HasMaxLength(200);
            b.Property(p => p.DocumentId).IsRequired().HasMaxLength(50);
            b.Property(p => p.CreatedAt).IsRequired();
            b.HasIndex(p => p.DocumentId).IsUnique();
        });

        // Vaccines/Vaccinations virão depois…
        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken ct = default)
        => base.SaveChangesAsync(ct); // ✅ agora é override
}

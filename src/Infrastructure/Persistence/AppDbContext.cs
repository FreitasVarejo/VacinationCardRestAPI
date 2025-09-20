using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Application.Abstractions;
using Domain.Entities;

namespace Infrastructure.Persistence
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        public DbSet<Person> People => Set<Person>();
        public DbSet<Vaccine> Vaccines => Set<Vaccine>();
        public DbSet<DoseSchedule> DoseSchedules => Set<DoseSchedule>();
        public DbSet<VaccinationCard> Cards => Set<VaccinationCard>();
        public DbSet<VaccinationEntry> Entries => Set<VaccinationEntry>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>()
                .HasOne(p => p.Card);

            modelBuilder.Entity<Vaccine>()
                .OwnsOne(v => v.Schedule);

            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken ct = default) => base.SaveChangesAsync(ct);
    }
}

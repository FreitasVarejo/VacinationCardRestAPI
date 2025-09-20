using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Application.Abstractions
{
    public interface IAppDbContext
    {
        DbSet<Person> People { get; }
        DbSet<Vaccine> Vaccines { get; }
        DbSet<DoseSchedule> DoseSchedules { get; }
        DbSet<VaccinationCard> Cards { get; }
        DbSet<VaccinationEntry> Entries { get; }

        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}

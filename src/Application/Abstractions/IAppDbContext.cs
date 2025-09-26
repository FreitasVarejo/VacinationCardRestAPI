using Microsoft.EntityFrameworkCore;
using VaccinationCard.Domain.Entities;

namespace VaccinationCard.Application.Abstractions;

public interface IAppDbContext
{
    DbSet<Person> Persons { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}

using MediatR;
using VaccinationCard.Application.Abstractions;
using VaccinationCard.Application.Persons;
using VaccinationCard.Application.Persons.Queries;
using Microsoft.EntityFrameworkCore;

namespace VaccinationCard.Application.Persons.Handlers;

public class ListPersonsHandler : IRequestHandler<ListPersonsQuery, IReadOnlyList<PersonDto>>
{
    private readonly IAppDbContext _db;
    public ListPersonsHandler(IAppDbContext db) => _db = db;

    public async Task<IReadOnlyList<PersonDto>> Handle(ListPersonsQuery request, CancellationToken ct)
    {
        var query = _db.Persons.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var s = request.Search.Trim();
            query = query.Where(p => p.Name.Contains(s) || p.DocumentId.Contains(s));
        }

        int skip = (request.Page - 1) * request.PageSize;
        return await query
            .OrderBy(p => p.Name)
            .Skip(skip)
            .Take(request.PageSize)
            .Select(p => new PersonDto(p.PersonId, p.Name, p.DocumentId, p.CreatedAt))
            .ToListAsync(ct);
    }
}

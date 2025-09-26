using MediatR;
using VaccinationCard.Application.Abstractions;
using VaccinationCard.Application.Persons;
using VaccinationCard.Application.Persons.Queries;
using Microsoft.EntityFrameworkCore;

namespace VaccinationCard.Application.Persons.Handlers;

public class GetPersonByIdHandler : IRequestHandler<GetPersonByIdQuery, PersonDto?>
{
    private readonly IAppDbContext _db;
    public GetPersonByIdHandler(IAppDbContext db) => _db = db;

    public async Task<PersonDto?> Handle(GetPersonByIdQuery request, CancellationToken ct)
    {
        return await _db.Persons.AsNoTracking()
            .Where(p => p.PersonId == request.PersonId)
            .Select(p => new PersonDto(p.PersonId, p.Name, p.DocumentId, p.CreatedAt))
            .FirstOrDefaultAsync(ct);
    }
}

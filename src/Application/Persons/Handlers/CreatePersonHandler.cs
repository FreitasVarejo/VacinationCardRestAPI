using MediatR;
using VaccinationCard.Application.Abstractions;
using VaccinationCard.Application.Persons;
using VaccinationCard.Application.Persons.Commands;
using VaccinationCard.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace VaccinationCard.Application.Persons.Handlers;

public class CreatePersonHandler : IRequestHandler<CreatePersonCommand, PersonDto>
{
    private readonly IAppDbContext _db;
    public CreatePersonHandler(IAppDbContext db) => _db = db;

    public async Task<PersonDto> Handle(CreatePersonCommand request, CancellationToken ct)
    {
        // normalização
        var name = request.Name.Trim();
        var doc  = request.DocumentId.Trim().ToUpperInvariant();

        // unicidade por DocumentId (normalizado)
        var exists = await _db.Persons.AsNoTracking()
            .AnyAsync(p => p.DocumentId == doc, ct);
        if (exists) throw new InvalidOperationException("DocumentId já cadastrado.");

        var entity = new Person(Guid.NewGuid(), name, doc);
        _db.Persons.Add(entity);
        await _db.SaveChangesAsync(ct);

        return new PersonDto(entity.PersonId, entity.Name, entity.DocumentId, entity.CreatedAt);
    }
}

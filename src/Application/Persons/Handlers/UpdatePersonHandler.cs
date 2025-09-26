using MediatR;
using VaccinationCard.Application.Abstractions;
using VaccinationCard.Application.Persons;
using VaccinationCard.Application.Persons.Commands;
using Microsoft.EntityFrameworkCore;

namespace VaccinationCard.Application.Persons.Handlers;

public class UpdatePersonHandler : IRequestHandler<UpdatePersonCommand, PersonDto>
{
    private readonly IAppDbContext _db;
    public UpdatePersonHandler(IAppDbContext db) => _db = db;

    public async Task<PersonDto> Handle(UpdatePersonCommand request, CancellationToken ct)
    {
        var entity = await _db.Persons.FirstOrDefaultAsync(p => p.PersonId == request.PersonId, ct);
        if (entity is null) throw new KeyNotFoundException("Pessoa não encontrada.");

        var name = request.Name.Trim();
        var doc  = request.DocumentId.Trim().ToUpperInvariant();

        // checar duplicidade contra outros registros
        var docTaken = await _db.Persons.AsNoTracking()
            .AnyAsync(p => p.DocumentId == doc && p.PersonId != request.PersonId, ct);
        if (docTaken) throw new InvalidOperationException("DocumentId já cadastrado para outra pessoa.");

        entity.Update(name, doc);
        await _db.SaveChangesAsync(ct);

        return new PersonDto(entity.PersonId, entity.Name, entity.DocumentId, entity.CreatedAt);
    }
}

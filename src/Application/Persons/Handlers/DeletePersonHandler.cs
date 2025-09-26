using MediatR;
using VaccinationCard.Application.Abstractions;
using VaccinationCard.Application.Persons.Commands;
using Microsoft.EntityFrameworkCore;

namespace VaccinationCard.Application.Persons.Handlers;

public class DeletePersonHandler : IRequestHandler<DeletePersonCommand>
{
    private readonly IAppDbContext _db;
    public DeletePersonHandler(IAppDbContext db) => _db = db;

    public async Task Handle(DeletePersonCommand request, CancellationToken ct)
    {
        var entity = await _db.Persons.FirstOrDefaultAsync(p => p.PersonId == request.PersonId, ct);
        if (entity is null) return;

        _db.Persons.Remove(entity);
        await _db.SaveChangesAsync(ct);
    }
}

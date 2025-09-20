using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.People.Commands.RemovePerson
{
    // ALTERE ESTA LINHA:
    // public class RemovePersonHandler : IRequestHandler<RemovePersonCommand>
    public class RemovePersonHandler : IRequestHandler<RemovePersonCommand, Unit>
    {
        private readonly IAppDbContext _db;
        public RemovePersonHandler(IAppDbContext db) => _db = db;

        public async Task<Unit> Handle(RemovePersonCommand request, CancellationToken ct)
        {
            var person = await _db.People
                .Include(p => p.Card).ThenInclude(c => c.Entries)
                .FirstOrDefaultAsync(p => p.Id == request.PersonId, ct);

            if (person != null)
            {
                if (person.Card?.Entries?.Count > 0)
                    _db.Entries.RemoveRange(person.Card.Entries);
                if (person.Card != null)
                    _db.Cards.Remove(person.Card);

                _db.People.Remove(person);
                await _db.SaveChangesAsync(ct);
            }
            return Unit.Value;
        }
    }
}

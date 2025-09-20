using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Card.Commands.DeleteEntry
{
    public class DeleteEntryHandler : IRequestHandler<DeleteEntryCommand>
    {
        private readonly IAppDbContext _db;
        public DeleteEntryHandler(IAppDbContext db) => _db = db;

        public async Task<Unit> Handle(DeleteEntryCommand request, CancellationToken ct)
        {
            var person = await _db.People.Include(p => p.Card)
                .ThenInclude(c => c.Entries)
                .FirstOrDefaultAsync(p => p.Id == request.PersonId, ct);

            if (person?.Card == null) return Unit.Value;

            var entry = person.Card.Entries.Find(e => e.Id == request.EntryId);
            if (entry != null)
            {
                person.Card.Entries.Remove(entry);
                _db.Entries.Remove(entry);
                await _db.SaveChangesAsync(ct);
            }
            return Unit.Value;
        }
    }
}

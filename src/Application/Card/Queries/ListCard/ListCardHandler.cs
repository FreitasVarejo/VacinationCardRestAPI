using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Application.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Card.Queries.ListCard
{
    public class ListCardHandler : IRequestHandler<ListCardQuery, IReadOnlyList<CardEntryDto>>
    {
        private readonly IAppDbContext _db;
        public ListCardHandler(IAppDbContext db) => _db = db;

        public async Task<IReadOnlyList<CardEntryDto>> Handle(ListCardQuery request, CancellationToken ct)
        {
            var person = await _db.People.Include(p => p.Card)
                .ThenInclude(c => c.Entries)
                .FirstOrDefaultAsync(p => p.Id == request.PersonId, ct);
            if (person == null || person.Card == null) return new List<CardEntryDto>();

            var vaccineLookup = await _db.Vaccines.ToDictionaryAsync(v => v.Id, ct);

            return person.Card.Entries
                .OrderBy(e => e.Date)
                .Select(e => new CardEntryDto
                {
                    EntryId = e.Id,
                    VaccineName = vaccineLookup.TryGetValue(e.VaccineId, out var v) ? v.Name : "Unknown",
                    VaccineCode = vaccineLookup.TryGetValue(e.VaccineId, out var v2) ? v2.Code : "",
                    Date = e.Date,
                    DoseNumber = e.DoseNumber,
                    LotNumber = e.LotNumber,
                    Notes = e.Notes
                }).ToList();
        }
    }
}

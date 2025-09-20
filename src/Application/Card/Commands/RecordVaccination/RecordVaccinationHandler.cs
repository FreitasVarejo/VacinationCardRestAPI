using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Card.Commands.RecordVaccination
{
    public class RecordVaccinationHandler : IRequestHandler<RecordVaccinationCommand, Guid>
    {
        private readonly IAppDbContext _db;
        public RecordVaccinationHandler(IAppDbContext db) => _db = db;

        public async Task<Guid> Handle(RecordVaccinationCommand request, CancellationToken ct)
        {
            var person = await _db.People.Include(p => p.Card)
                .FirstOrDefaultAsync(p => p.Id == request.PersonId, ct);
            if (person == null || person.Card == null) throw new Exception("Person not found");

            var vaccine = await _db.Vaccines.Include(v => v.Schedule)
                .FirstOrDefaultAsync(v => v.Id == request.VaccineId, ct);
            if (vaccine == null) throw new Exception("Vaccine not found");

            // entradas anteriores dessa vacina
            var prior = await _db.Entries
                .Where(e => person.Card.Id == _db.Cards.First(c => c.Id == person.Card.Id).Id && e.VaccineId == request.VaccineId)
                .OrderBy(e => e.DoseNumber)
                .ToListAsync(ct);

            // Validação simples: dose válida e respeita intervalo mínimo básico (se houver)
            if (!vaccine.Schedule.ValidDoseNumbers.Contains(request.DoseNumber))
                throw new Exception("Invalid dose number for schedule");

            if (request.DoseNumber > 1 && prior.Any())
            {
                var previous = prior.Last();
                if (vaccine.Schedule.MinIntervalDays.TryGetValue(previous.DoseNumber, out var minDays))
                {
                    var delta = (request.Date.Date - previous.Date.Date).TotalDays;
                    if (delta < minDays)
                        throw new Exception($"Minimum interval of {minDays} days not met");
                }
            }

            var entry = new VaccinationEntry
            {
                Id = Guid.NewGuid(),
                VaccineId = request.VaccineId,
                Date = request.Date,
                DoseNumber = request.DoseNumber,
                LotNumber = request.LotNumber,
                Notes = request.Notes
            };

            person.Card.Entries.Add(entry);
            _db.Entries.Add(entry);
            await _db.SaveChangesAsync(ct);

            return entry.Id;
        }
    }
}

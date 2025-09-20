using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions;
using Domain.Entities;
using MediatR;

namespace Application.Vaccines.Commands.RegisterVaccine
{
    public class RegisterVaccineHandler : IRequestHandler<RegisterVaccineCommand, Guid>
    {
        private readonly IAppDbContext _db;

        public RegisterVaccineHandler(IAppDbContext db) => _db = db;

        public async Task<Guid> Handle(RegisterVaccineCommand request, CancellationToken ct)
        {
            var schedule = new DoseSchedule
            {
                Id = Guid.NewGuid(),
                TotalDoses = request.TotalDoses
            };
            for (var i = 1; i <= request.TotalDoses; i++)
                schedule.ValidDoseNumbers.Add(i);

            var vaccine = new Vaccine
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Code = request.Code,
                Schedule = schedule
            };

            _db.DoseSchedules.Add(schedule);
            _db.Vaccines.Add(vaccine);
            await _db.SaveChangesAsync(ct);
            return vaccine.Id;
        }
    }
}

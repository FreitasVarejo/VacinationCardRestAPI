using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions;
using Domain.Entities;
using MediatR;

namespace Application.People.Commands.RegisterPerson
{
    public class RegisterPersonHandler : IRequestHandler<RegisterPersonCommand, Guid>
    {
        private readonly IAppDbContext _db;
        public RegisterPersonHandler(IAppDbContext db) => _db = db;

        public async Task<Guid> Handle(RegisterPersonCommand request, CancellationToken ct)
        {
            var person = new Person
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                DocNumber = request.DocNumber,
                Card = new VaccinationCard { Id = Guid.NewGuid() }
            };

            _db.People.Add(person);
            _db.Cards.Add(person.Card);
            await _db.SaveChangesAsync(ct);
            return person.Id;
        }
    }
}

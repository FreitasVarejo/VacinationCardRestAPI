using MediatR;

namespace VaccinationCard.Application.Persons.Commands;

public record UpdatePersonCommand(Guid PersonId, string Name, string DocumentId) : IRequest<PersonDto>;

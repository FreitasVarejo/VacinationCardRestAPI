using MediatR;

namespace VaccinationCard.Application.Persons.Commands;

public record CreatePersonCommand(string Name, string DocumentId) : IRequest<PersonDto>;

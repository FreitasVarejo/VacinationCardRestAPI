using MediatR;

namespace VaccinationCard.Application.Persons.Commands;

public record DeletePersonCommand(Guid PersonId) : IRequest;

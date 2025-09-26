using MediatR;

namespace VaccinationCard.Application.Persons.Queries;

public record GetPersonByIdQuery(Guid PersonId) : IRequest<PersonDto?>;

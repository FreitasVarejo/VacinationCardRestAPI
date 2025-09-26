using MediatR;

namespace VaccinationCard.Application.Persons.Queries;

public record ListPersonsQuery(string? Search, int Page = 1, int PageSize = 20)
    : IRequest<IReadOnlyList<PersonDto>>;

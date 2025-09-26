using FluentValidation;
using VaccinationCard.Application.Persons.Queries;

namespace VaccinationCard.Application.Persons.Validation;

public class GetPersonByIdValidator : AbstractValidator<GetPersonByIdQuery>
{
    public GetPersonByIdValidator() => RuleFor(x => x.PersonId).NotEmpty();
}

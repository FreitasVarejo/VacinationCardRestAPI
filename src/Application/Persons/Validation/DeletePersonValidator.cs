using FluentValidation;
using VaccinationCard.Application.Persons.Commands;

namespace VaccinationCard.Application.Persons.Validation;

public class DeletePersonValidator : AbstractValidator<DeletePersonCommand>
{
    public DeletePersonValidator() => RuleFor(x => x.PersonId).NotEmpty();
}

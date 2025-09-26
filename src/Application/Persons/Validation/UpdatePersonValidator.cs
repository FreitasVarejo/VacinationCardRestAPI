using FluentValidation;
using VaccinationCard.Application.Persons.Commands;

namespace VaccinationCard.Application.Persons.Validation;

public class UpdatePersonValidator : AbstractValidator<UpdatePersonCommand>
{
    public UpdatePersonValidator()
    {
        RuleFor(x => x.PersonId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.DocumentId).NotEmpty().MaximumLength(50);
    }
}

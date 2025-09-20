using FluentValidation;

namespace Application.Card.Commands.RecordVaccination
{
    public class RecordVaccinationValidator : AbstractValidator<RecordVaccinationCommand>
    {
        public RecordVaccinationValidator()
        {
            RuleFor(x => x.PersonId).NotEmpty();
            RuleFor(x => x.VaccineId).NotEmpty();
            RuleFor(x => x.Date).LessThanOrEqualTo(System.DateTime.UtcNow.AddDays(1)); // data não futura
            RuleFor(x => x.DoseNumber).GreaterThanOrEqualTo(1);
            RuleFor(x => x.LotNumber).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Notes).MaximumLength(500);
        }
    }
}

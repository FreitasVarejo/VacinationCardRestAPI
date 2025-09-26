namespace VaccinationCard.Application.Persons;

public record PersonDto(Guid PersonId, string Name, string DocumentId, DateTime CreatedAt);

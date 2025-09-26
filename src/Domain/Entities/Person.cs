namespace VaccinationCard.Domain.Entities;

public class Person
{
    // EF precisa de ctor vazio
    private Person() { }

    public Person(Guid personId, string name, string documentId)
    {
        PersonId = personId == default ? Guid.NewGuid() : personId;
        Name = name?.Trim() ?? throw new ArgumentNullException(nameof(name));
        DocumentId = documentId?.Trim() ?? throw new ArgumentNullException(nameof(documentId));
        CreatedAt = DateTime.UtcNow;
    }

    public Guid PersonId { get; private set; }
    public string Name { get; private set; } = null!;
    public string DocumentId { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }

    public void Update(string name, string documentId)
    {
        Name = string.IsNullOrWhiteSpace(name) ? Name : name.Trim();
        DocumentId = string.IsNullOrWhiteSpace(documentId) ? DocumentId : documentId.Trim();
    }
}

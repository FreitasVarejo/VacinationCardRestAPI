using System;

namespace Domain.Entities
{
    public class Person
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string DocNumber { get; set; } = string.Empty;

        public VaccinationCard Card { get; set; } = new VaccinationCard { Id = Guid.NewGuid() };
    }
}

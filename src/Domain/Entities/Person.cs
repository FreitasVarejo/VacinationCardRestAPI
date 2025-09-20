using System;

namespace Domain.Entities
{
    public class Person
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DocNumber { get; set; }

        public VaccinationCard Card { get; set; } // 1-1
    }
}

using System;

namespace Domain.Entities
{
    public class Vaccine
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; } // ex. "MMR", "COVID19"
        public DoseSchedule Schedule { get; set; }
    }
}

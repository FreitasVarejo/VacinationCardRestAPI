using System;

namespace Domain.Entities
{
    public class Vaccine
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public DoseSchedule Schedule { get; set; } = new DoseSchedule { Id = Guid.NewGuid() };
    }
}

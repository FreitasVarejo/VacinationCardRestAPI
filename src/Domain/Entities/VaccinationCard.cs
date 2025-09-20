using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class VaccinationCard
    {
        public Guid Id { get; set; }
        public Guid PersonId { get; set; }
        public List<VaccinationEntry> Entries { get; set; } = new();
    }
}

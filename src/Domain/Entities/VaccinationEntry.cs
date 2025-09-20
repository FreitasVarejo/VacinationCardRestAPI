using System;

namespace Domain.Entities
{
    public class VaccinationEntry
    {
        public Guid Id { get; set; }
        public Guid VaccineId { get; set; }
        public DateTime Date { get; set; }
        public int DoseNumber { get; set; }
        public string LotNumber { get; set; }
        public string Notes { get; set; }
    }
}

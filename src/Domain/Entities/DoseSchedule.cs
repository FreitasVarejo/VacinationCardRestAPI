using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class DoseSchedule
    {
        public Guid Id { get; set; }
        public int TotalDoses { get; set; }
        public List<int> ValidDoseNumbers { get; set; } = new();
        // minIntervalDays: chave = doseAnterior -> dias mínimos até a dose atual
        public Dictionary<int,int> MinIntervalDays { get; set; } = new();
    }
}

using System;
using MediatR;

namespace Application.Card.Commands.RecordVaccination
{
    public class RecordVaccinationCommand : IRequest<Guid>
    {
        public Guid PersonId { get; set; }
        public Guid VaccineId { get; set; }
        public DateTime Date { get; set; }
        public int DoseNumber { get; set; }
        public string LotNumber { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
    }
}

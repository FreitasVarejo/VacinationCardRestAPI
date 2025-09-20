using System;
using System.Collections.Generic;
using MediatR;

namespace Application.Card.Queries.ListCard
{
    public class ListCardQuery : IRequest<IReadOnlyList<CardEntryDto>>
    {
        public Guid PersonId { get; set; }
    }

    public class CardEntryDto
    {
        public Guid EntryId { get; set; }
        public string VaccineName { get; set; }
        public string VaccineCode { get; set; }
        public DateTime Date { get; set; }
        public int DoseNumber { get; set; }
        public string LotNumber { get; set; }
        public string Notes { get; set; }
    }
}

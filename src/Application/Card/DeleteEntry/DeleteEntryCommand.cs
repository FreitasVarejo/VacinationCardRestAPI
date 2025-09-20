using System;
using MediatR;

namespace Application.Card.Commands.DeleteEntry
{
    public class DeleteEntryCommand : IRequest
    {
        public Guid PersonId { get; set; }
        public Guid EntryId { get; set; }
    }
}

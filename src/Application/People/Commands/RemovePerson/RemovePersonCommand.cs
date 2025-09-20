using System;
using MediatR;

namespace Application.People.Commands.RemovePerson
{
    public class RemovePersonCommand : IRequest
    {
        public Guid PersonId { get; set; }
    }
}

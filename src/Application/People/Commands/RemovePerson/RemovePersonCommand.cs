using System;
using MediatR;

namespace Application.People.Commands.RemovePerson
{
    public class RemovePersonCommand : IRequest<Unit>
    {
        public Guid PersonId { get; set; }
    }
}

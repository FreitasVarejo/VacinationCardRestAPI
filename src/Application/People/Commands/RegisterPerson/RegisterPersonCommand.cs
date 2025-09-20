using System;
using MediatR;

namespace Application.People.Commands.RegisterPerson
{
    public class RegisterPersonCommand : IRequest<Guid>
    {
        public string Name { get; set; }
        public string DocNumber { get; set; }
    }
}

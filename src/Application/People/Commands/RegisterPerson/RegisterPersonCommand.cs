using System;
using MediatR;

namespace Application.People.Commands.RegisterPerson
{
    public class RegisterPersonCommand : IRequest<Guid>
    {
        public string Name { get; set; } = string.Empty;
        public string DocNumber { get; set; } = string.Empty;
    }
}

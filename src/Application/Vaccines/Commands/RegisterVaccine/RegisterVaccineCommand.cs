using System;
using MediatR;

namespace Application.Vaccines.Commands.RegisterVaccine
{
    public class RegisterVaccineCommand : IRequest<Guid>
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public int TotalDoses { get; set; }
    }
}

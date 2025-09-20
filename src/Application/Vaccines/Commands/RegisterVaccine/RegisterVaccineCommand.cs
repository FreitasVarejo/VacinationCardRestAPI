using System;
using MediatR;

namespace Application.Vaccines.Commands.RegisterVaccine
{
    public class RegisterVaccineCommand : IRequest<Guid>
    {
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public int TotalDoses { get; set; }
    }
}

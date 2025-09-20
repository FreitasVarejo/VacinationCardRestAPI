using Application.Vaccines.Commands.RegisterVaccine;
using System;
using System.Collections.Generic; // se retornar listas
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/vaccines")]
    public class VaccinesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public VaccinesController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        public async Task<ActionResult<Guid>> Create(RegisterVaccineCommand cmd)
        {
            var id = await _mediator.Send(cmd);
            return Ok(id);
        }
    }
}

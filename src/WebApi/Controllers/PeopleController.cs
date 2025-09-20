using Application.People.Commands.RegisterPerson;
using Application.People.Commands.RemovePerson;
using System;
using System.Collections.Generic; // se retornar listas
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/people")]
    public class PeopleController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PeopleController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        public async Task<ActionResult<Guid>> Create(RegisterPersonCommand cmd)
        {
            var id = await _mediator.Send(cmd);
            return Ok(id);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new RemovePersonCommand { PersonId = id });
            return NoContent();
        }
    }
}

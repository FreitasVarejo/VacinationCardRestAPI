using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Card.Commands.DeleteEntry;
using Application.Card.Commands.RecordVaccination;
using Application.Card.Queries.ListCard;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/people/{personId:guid}/card")]
    public class CardController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CardController(IMediator mediator) => _mediator = mediator;

        [HttpPost("entries")]
        public async Task<ActionResult<Guid>> Record(Guid personId, RecordVaccinationCommand cmd)
        {
            cmd.PersonId = personId;
            var entryId = await _mediator.Send(cmd);
            return Ok(entryId);
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<CardEntryDto>>> List(Guid personId)
        {
            var list = await _mediator.Send(new ListCardQuery { PersonId = personId });
            return Ok(list);
        }

        [HttpDelete("entries/{entryId:guid}")]
        public async Task<IActionResult> Delete(Guid personId, Guid entryId)
        {
            await _mediator.Send(new DeleteEntryCommand { PersonId = personId, EntryId = entryId });
            return NoContent();
        }
    }
}

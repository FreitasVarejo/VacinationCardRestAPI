using MediatR;
using Microsoft.AspNetCore.Mvc;
using VaccinationCard.Application.Persons;
using VaccinationCard.Application.Persons.Commands;
using VaccinationCard.Application.Persons.Queries;

namespace VaccinationCard.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonsController : ControllerBase
{
    private readonly IMediator _mediator;
    public PersonsController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<ActionResult<PersonDto>> Create([FromBody] CreatePersonCommand cmd, CancellationToken ct)
    {
        var dto = await _mediator.Send(cmd, ct);
        return CreatedAtAction(nameof(GetById), new { id = dto.PersonId }, dto);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PersonDto>> GetById([FromRoute] Guid id, CancellationToken ct)
    {
        var dto = await _mediator.Send(new GetPersonByIdQuery(id), ct);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<PersonDto>>> List([FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
    {
        var list = await _mediator.Send(new ListPersonsQuery(search, page, pageSize), ct);
        return Ok(list);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<PersonDto>> Update([FromRoute] Guid id, [FromBody] UpdatePersonCommand body, CancellationToken ct)
    {
        if (id != body.PersonId) return BadRequest("Route id != body.PersonId");
        var dto = await _mediator.Send(body, ct);
        return Ok(dto);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken ct)
    {
        await _mediator.Send(new DeletePersonCommand(id), ct);
        return NoContent();
    }
}

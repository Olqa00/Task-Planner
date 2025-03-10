namespace Planner.WebApi.Controllers;

using Planner.Application.Commands;
using Planner.Application.Queries;
using Planner.Application.Results;

[ApiController, Route("[controller]")]
public sealed class TasksController : ControllerBase
{
    private readonly ISender mediator;

    public TasksController(ISender mediator)
    {
        this.mediator = mediator;
    }

    [HttpPost, Route("AddTask")]
    public async Task<IActionResult> AddTask([FromBody] AddTask command, CancellationToken cancellationToken)
    {
        await this.mediator.Send(command, cancellationToken);

        return this.Accepted();
    }

    [HttpDelete, Route("DeleteTask")]
    public async Task<IActionResult> DeleteTask([FromQuery] Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteTask
        {
            Id = id,
        };

        await this.mediator.Send(command, cancellationToken);

        return this.Accepted();
    }

    [HttpGet, Route("GetTask")]
    public async Task<TaskResult?> GetTask([FromQuery] Guid id, CancellationToken cancellationToken)
    {
        var query = new GetTask
        {
            Id = id,
        };

        return await this.mediator.Send(query, cancellationToken);
    }

    [HttpGet, Route("GetTasks")]
    public async Task<IReadOnlyList<TaskResult>> GetTasks(CancellationToken cancellationToken)
    {
        var query = new GetTasks();

        return await this.mediator.Send(query, cancellationToken);
    }

    [HttpPut, Route("UpdateTask")]
    public async Task<IActionResult> UpdateTask([FromBody] UpdateTask command, CancellationToken cancellationToken)
    {
        await this.mediator.Send(command, cancellationToken);

        return this.Accepted();
    }
}

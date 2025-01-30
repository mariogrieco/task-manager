using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Core.Entities;
using TaskManager.Core.Interfaces;

namespace TaskManager.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITaskRepository _taskRepo;

    public TasksController(ITaskRepository taskRepo)
    {
        _taskRepo = taskRepo;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        if (HttpContext.Items["User"] is not User user)
        {
            return Unauthorized();
        }
        var tasks = await _taskRepo.GetAllAsync(user.Id);
        return Ok(tasks);
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        User? user = HttpContext.Items["User"] as User;
        if (user == null)
        {
            return Unauthorized();
        }
        var task = await _taskRepo.GetByIdAsync(id, user.Id);
        return task != null ? Ok(task) : NotFound();
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(TaskItem task)
    {
        User? user = HttpContext.Items["User"] as User;
        if (user == null)
        {
            return Unauthorized();
        }
        task.Id = Guid.NewGuid();
        task.CreatedAt = DateTime.UtcNow;
        task.UpdatedAt = DateTime.UtcNow;
        task.UserId = user.Id;

        await _taskRepo.AddAsync(task, user.Id);
        return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, TaskItem task)
    {
        if (id != task.Id) return BadRequest("ID mismatch");

        User? user = HttpContext.Items["User"] as User;
        if (user == null)
        {
            return Unauthorized();
        }
        var existingTask = await _taskRepo.GetByIdAsync(id, user.Id);
        if (existingTask == null) return NotFound();

        task.UpdatedAt = DateTime.UtcNow;
        await _taskRepo.UpdateAsync(task, user.Id);
        return NoContent();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        if (HttpContext.Items["User"] is not User user)
        {
            return Unauthorized();
        }
        await _taskRepo.DeleteAsync(id, user.Id);
        return NoContent();
    }
}

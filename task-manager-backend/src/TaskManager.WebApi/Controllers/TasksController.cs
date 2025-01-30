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

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var tasks = await _taskRepo.GetAllAsync();
        return Ok(tasks);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var task = await _taskRepo.GetByIdAsync(id);
        return task != null ? Ok(task) : NotFound();
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(TaskItem task)
    {
        task.Id = Guid.NewGuid();
        task.CreatedAt = DateTime.UtcNow;
        task.UpdatedAt = DateTime.UtcNow;
        
        await _taskRepo.AddAsync(task);
        return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, TaskItem task)
    {
        if (id != task.Id) return BadRequest("ID mismatch");
        
        var existingTask = await _taskRepo.GetByIdAsync(id);
        if (existingTask == null) return NotFound();
        
        await _taskRepo.UpdateAsync(task);
        return NoContent();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _taskRepo.DeleteAsync(id);
        return NoContent();
    }
}

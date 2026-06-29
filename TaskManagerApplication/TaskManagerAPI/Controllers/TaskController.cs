using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.DTOs;
using TaskManagerAPI.Services;

namespace TaskManagerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TaskController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TaskController(
        ITaskService taskService
    )
    {
        _taskService = taskService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskResponseDTO>>> GetAllTasks()
    {
        var tasks = await _taskService.GetAllTasksAsync();

        return Ok(tasks);
    }
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<TaskResponseDTO>> GetTaskById (int id)
    {
        var task = await _taskService.GetTaskByIdAsync(id);

        return Ok(task);
    }

    [HttpPost]
    public async Task<ActionResult<TaskResponseDTO>> CreateTask ([FromBody] CreateTaskRequestDTO dto)
    {
        var createdTask = await _taskService.CreateTaskAsync(dto);

        return CreatedAtAction(
            nameof(GetTaskById),
            new { id = createdTask.Id },
            createdTask
        );
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<TaskResponseDTO>> UpdateTask
    (
        int id,
        [FromBody] UpdateTaskRequestDTO dto
    )
    {
        var updatedTask = await _taskService.UpdateTaskAsync(id, dto);

        return Ok(updatedTask);
    }

    [HttpPatch("{id:int}/toggle")]
    public async Task<ActionResult<TaskResponseDTO>> ToggleTask (int id)
    {
        var updatedTask = await _taskService.ToggleTaskAsync(id);

        return Ok(updatedTask);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteTask (int id)
    {
        await _taskService.DeleteTaskAsync(id);

        return NoContent();
    }
}

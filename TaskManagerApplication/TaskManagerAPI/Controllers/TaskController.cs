using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.DTOs;
using TaskManagerAPI.Services;

namespace TaskManagerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public BooksController(
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
    public async Task<ActionResult<TaskResponseDTO>> GetTaskById
    (
        int id, 
        [FromBody] TaskRequestDTO dto
    )
    {
        var task = await _taskService.GetTaskByIdAsync(id, dto);

        return Ok(task);
    }

    [HttpPost]
    public async Task<ActionResult<TaskResponseDTO>> CreateTask ([FromBody] TaskRequestDTO dto)
    {
        var createdTask = await _taskService.CreateTaskAsync(dto);

        return CreatedAtAction(
            nameof(GetTaskById),
            new { id = createdTask.Id },
            createdTask
        );
    }

    [HttpPut("{id: int}")]
    public async Task<ActionResult<TaskResponseDTO>> UpdateTask
    (
        int id,
        [FromBody] TaskRequestDTO dto
    )
    {
        var updatedTask = await _taskService.UpdateTaskAsync(id, dto);

        return Ok(updatedTask);
    }

    [HttpPatch("{id: int}/toggle")]
    public async Task<ActionResult<TaskResponseDTO>> ToggleTask
    (
        int id,
        [FromBody] TaskRequestDTO dto
    )
    {
        var updatedTask = await _taskService.ToggleTaskAsync(id, dto);

        return Ok(updatedTask);
    }

    [HttpDelete("{id: int}")]
    public async Task<IActionResult> DeleteTask
    (
        int id,
        [FromBody] TaskRequestDTO dto
    )
    {
        await _taskService.DeleteTaskAsync(id, dto);

        return NoContent();
    }
}

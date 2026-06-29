using TaskManagerAPI.Data;
using TaskManagerAPI.DTOs;
using TaskManagerAPI.Exceptions;
using TaskManagerAPI.Mappers;

namespace TaskManagerAPI.Services;

public class TaskService : ITaskService
{
    public Task <IEnumerable<TaskResponseDTO>> GetAllTasksAsync()
    {
        var tasks = InMemoryStore.Tasks.Select(TaskMapper.ToResponse);

        return Task.FromResult(tasks);
    }

    public Task<TaskResponseDTO> GetTaskByIdAsync(int taskId, TaskRequestDTO dto)
    {
        var task = InMemoryStore.Tasks.FirstOrDefault(t => t.Id == taskId);

        if (task is null)
        {
            throw new TaskNotFoundException(dto.Name);
        }

        return Task.FromResult(TaskMapper.ToResponse(task));
    }

    public Task<TaskResponseDTO> CreateTaskAsync(TaskRequestDTO dto)
    {
        var task = TaskMapper.ToEntity(dto);

        task.Id = InMemoryStore.Tasks.Any()
            ? InMemoryStore.Tasks.Max(t => t.Id) + 1
            : 1;
        
        task.CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow);

        return Task.FromResult(TaskMapper.ToResponse(task));
    }

    public Task<TaskResponseDTO> UpdateTaskAsync(int taskId, TaskRequestDTO dto)
    {
        var task = InMemoryStore.Tasks.FirstOrDefault(t => t.Id == taskId);

        if (task is null)
        {
            throw new TaskNotFoundException(dto.Name);
        }

        task.Name = dto.Name;
        task.Description = dto.Description;
        task.Completed = dto.Completed;
        task.Priority = dto.Priority;
        task.DueDate = dto.DueDate;

        return Task.FromResult(TaskMapper.ToResponse(task));
    }

    public Task<TaskResponseDTO> ToggleTaskAsync(int taskId, TaskRequestDTO dto)
    {
        var task = InMemoryStore.Tasks.FirstOrDefault(t => t.Id == taskId);

        if (task is null)
        {
            throw new TaskNotFoundException(dto.Name);
        }

        task.Completed = !task.Completed;

        return Task.FromResult(TaskMapper.ToResponse(task));
    }

    public Task DeleteTaskAsync(int taskId, TaskRequestDTO dto)
    {
        var task = InMemoryStore.Tasks.FirstOrDefault(t => t.Id == taskId);

        if (task is null)
        {
            throw new TaskNotFoundException(dto.Name);
        }

        InMemoryStore.Tasks.Remove(task);

        return Task.CompletedTask;
    }
}

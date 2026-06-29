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

    public Task<TaskResponseDTO> GetTaskByIdAsync(int taskId)
    {
        var task = InMemoryStore.Tasks.FirstOrDefault(t => t.Id == taskId);

        if (task is null)
        {
            throw new TaskNotFoundException(taskId);
        }

        return Task.FromResult(TaskMapper.ToResponse(task));
    }

    public Task<TaskResponseDTO> CreateTaskAsync(CreateTaskRequestDTO dto)
    {
        var task = TaskMapper.ToEntity(dto);

        task.Id = InMemoryStore.Tasks.Any()
            ? InMemoryStore.Tasks.Max(t => t.Id) + 1
            : 1;
        
        task.CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow);

        InMemoryStore.Tasks.Add(task);

        return Task.FromResult(TaskMapper.ToResponse(task));
    }

    public Task<TaskResponseDTO> UpdateTaskAsync(int taskId, UpdateTaskRequestDTO dto)
    {
        var task = InMemoryStore.Tasks.FirstOrDefault(t => t.Id == taskId);

        if (task is null)
        {
            throw new TaskNotFoundException(taskId);
        }

        TaskMapper.UpdateEntity(task, dto);

        return Task.FromResult(TaskMapper.ToResponse(task));
    }

    public Task<TaskResponseDTO> ToggleTaskAsync(int taskId)
    {
        var task = InMemoryStore.Tasks.FirstOrDefault(t => t.Id == taskId);

        if (task is null)
        {
            throw new TaskNotFoundException(taskId);
        }

        task.Completed = !task.Completed;

        return Task.FromResult(TaskMapper.ToResponse(task));
    }

    public Task DeleteTaskAsync(int taskId)
    {
        var task = InMemoryStore.Tasks.FirstOrDefault(t => t.Id == taskId);

        if (task is null)
        {
            throw new TaskNotFoundException(taskId);
        }

        InMemoryStore.Tasks.Remove(task);

        return Task.CompletedTask;
    }
}

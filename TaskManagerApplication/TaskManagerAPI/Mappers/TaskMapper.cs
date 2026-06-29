using TaskManagerAPI.Models;
using TaskManagerAPI.DTOs;

namespace TaskManagerAPI.Mappers;

public static class TaskMapper
{
    public static TaskEntity ToEntity(CreateTaskRequestDTO dto)
    {
        return new TaskEntity
        {
            Name = dto.Name,
            Description = dto.Description,
            Priority = dto.Priority,
            DueDate = dto.DueDate
        };
    }

    public static void UpdateEntity(TaskEntity task, UpdateTaskRequestDTO dto)
    {
        task.Name = dto.Name;
        task.Description = dto.Description;
        task.Completed = dto.Completed;
        task.Priority = dto.Priority;
        task.DueDate = dto.DueDate;
    }

    public static TaskResponseDTO ToResponse(TaskEntity task)
    {
        return new TaskResponseDTO
        {
            Id = task.Id,
            Name = task.Name,
            Description = task.Description,
            Completed = task.Completed,
            Priority = task.Priority,
            DueDate = task.DueDate,
            CreatedAt = task.CreatedAt
        };
    }
}
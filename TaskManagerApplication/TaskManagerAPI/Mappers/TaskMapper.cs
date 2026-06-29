using TaskManagerAPI.Models;
using TaskManagerAPI.DTOs;

namespace TaskManagerAPI.Mappers;

public static class TaskMapper
{
    public static TaskEntity ToEntity(TaskRequestDTO dto)
    {
        return new TaskEntity
        {
            Id = dto.Id,
            Name = dto.Name,
            Description = dto.Description,
            Completed = dto.Completed,
            Priority = dto.Priority,
            DueDate = dto.DueDate,
            CreatedAt = dto.CreatedAt
        };
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
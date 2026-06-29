using System.ComponentModel.DataAnnotations;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.DTOs;

public class TaskResponseDTO
{
    public string Name { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    public bool Completed { get; init; }

    public Priority Priority { get; init; }

    public DateOnly DueDate { get; init; }

    public DateOnly CreatedAt { get; init; }
}

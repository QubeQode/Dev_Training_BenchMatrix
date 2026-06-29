using System.ComponentModel.DataAnnotations;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.DTOs;

public class TaskRequestDTO
{
    [Required]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    public bool Completed { get; set; }

    [EnumDataType(typeof(Priority))]
    public Priority Priority { get; set; }

    [Required]
    public DateOnly DueDate { get; set; }

    [Required]
    public DateOnly CreatedAt { get; set; }
}

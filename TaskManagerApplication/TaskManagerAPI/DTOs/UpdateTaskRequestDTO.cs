using System.ComponentModel.DataAnnotations;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.DTOs;

public class UpdateTaskRequestDTO
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    [Required]
    public bool Completed { get; set; }

    [Required]
    public Priority Priority { get; set; }

    [Required]
    public DateOnly DueDate { get; set; }
}

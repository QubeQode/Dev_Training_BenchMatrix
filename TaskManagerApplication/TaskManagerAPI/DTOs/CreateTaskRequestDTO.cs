using System.ComponentModel.DataAnnotations;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.DTOs;

public class CreateTaskRequestDTO
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    [EnumDataType(typeof(Priority))]
    public Priority Priority { get; set; }

    [Required]
    public DateOnly DueDate { get; set; }
}

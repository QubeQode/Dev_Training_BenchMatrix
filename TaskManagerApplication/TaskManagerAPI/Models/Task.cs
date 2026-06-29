namespace TaskManagerAPI.Models;

public enum Priority
{
    Low,
    Medium,
    High
}

public class TaskEntity
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public bool Completed { get; set; } = false;

    public Priority Priority { get; set; }

    public DateOnly DueDate { get; set; }

    public DateOnly CreatedAt { get; set; }
}

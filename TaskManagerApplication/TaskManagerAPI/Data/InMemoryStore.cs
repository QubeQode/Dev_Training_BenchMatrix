using TaskManagerAPI.Models;

namespace TaskManagerAPI.Data;

public class InMemoryStore
{
    public static List<TaskEntity> Tasks { get; } =
    [
        new TaskEntity
        {
            Id = 1,
            Name = "Implement Task Creation Endpoint",
            Description = "Create the POST endpoint for adding new tasks to the API.",
            Completed = true,
            Priority = Priority.High,
            DueDate = new DateOnly(2026, 7, 5),
            CreatedAt = new DateOnly(2026, 6, 25)
        },
        new TaskEntity
        {
            Id = 2,
            Name = "Add Task Validation",
            Description = "Implement validation rules for incoming task requests.",
            Completed = false,
            Priority = Priority.High,
            DueDate = new DateOnly(2026, 7, 3),
            CreatedAt = new DateOnly(2026, 6, 27)
        },
        new TaskEntity
        {
            Id = 3,
            Name = "Write Unit Tests",
            Description = "Create unit tests for service and mapper classes.",
            Completed = false,
            Priority = Priority.Medium,
            DueDate = new DateOnly(2026, 7, 10),
            CreatedAt = new DateOnly(2026, 6, 28)
        },
        new TaskEntity
        {
            Id = 4,
            Name = "Update API Documentation",
            Description = "Document all task management endpoints and request models.",
            Completed = false,
            Priority = Priority.Low,
            DueDate = new DateOnly(2026, 7, 15),
            CreatedAt = new DateOnly(2026, 6, 29)
        },
        new TaskEntity
        {
            Id = 5,
            Name = "Refactor Repository Layer",
            Description = "Clean up repository code and improve readability.",
            Completed = true,
            Priority = Priority.Medium,
            DueDate = new DateOnly(2026, 7, 1),
            CreatedAt = new DateOnly(2026, 6, 20)
        }
    ];
}

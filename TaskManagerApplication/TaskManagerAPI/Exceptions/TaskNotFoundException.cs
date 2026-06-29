namespace TaskManagerAPI.Exceptions;
public class TaskNotFoundException : TaskException
{
    public TaskNotFoundException(int taskId) 
        : base($"Task {taskId} was not found."){}
}
namespace TaskManagerAPI.Exceptions;
public class TaskNotFoundException : TaskException
{
    public TaskNotFoundException(string taskName) 
        : base($"{taskName} task was not found."){}
}
namespace TaskManagerAPI.Exceptions;

public abstract class TaskException : Exception
{
    protected TaskException(string message) : base(message) {}
}

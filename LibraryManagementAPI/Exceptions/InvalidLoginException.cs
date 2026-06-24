// Invalid login is before JWT protection so needs to be handled with custom exception

namespace LibraryManagementAPI.Exceptions;

public class InvalidLoginException : Exception
{
    public InvalidLoginException()
        : base("Invalid username or password.")
    {
    }
}

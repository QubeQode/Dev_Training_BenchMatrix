using System.Security.AccessControl;

namespace LibraryManagementAPI.Models;

public class Book
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public int Year { get; set; }

    public int PageCount { get; set; }

    public string? Isbn { get; set; } // Nullable because seed data doesn't have Isbn

    public int AuthorId { get; set; }

    public Author? Author { get; set; }
}

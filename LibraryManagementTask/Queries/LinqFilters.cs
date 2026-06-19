using LibraryManagementTask.Models;
namespace LibraryManagementTask.Queries;

public class LinqFilters
{
    private readonly IEnumerable<Book> _books;
    private readonly IEnumerable<Author> _authors;

    public LinqFilters(IEnumerable<Book> books, IEnumerable<Author> authors)
    {
        _books = books;
        _authors = authors;
    }

    public void FilterByAuthor(string authorName)
    {
        var books = _books
            .Where(b => b.Author?.Name == authorName);
        
        Console.WriteLine($"\nBooks by {authorName}:");

        foreach (var book in books)
            Console.WriteLine(book.Title);
    }

    public void SortTitles()
    {
        var titles = _books
            .Select(b => b.Title)
            .OrderBy(t => t);
        
        Console.WriteLine("\nSorted Titles:");

        foreach (var title in titles)
            Console.WriteLine(title);
    }

    public void GroupByAuthor()
    {
        var groupedBooks = _books
            .GroupBy(b => b.Author?.Name ?? "Unknown")
            .Select(group => new
            {
                Author = group.Key,
                Titles = string.Join("\n",
                    group.Select(book => $" {book.Title}"))
            });
        
        Console.WriteLine("\nBooks Grouped By Author:");

        foreach (var group in groupedBooks)
        {
            Console.WriteLine($"\n{group.Author}:\n{group.Titles}");
        }
    }

    public void AveragePages()
    {
        double avg = _books
            .Average(b => b.PageCount);
        
        Console.WriteLine($"\nAverage Pages: {avg:F2}");
    }

    public void AnyOverFiveHundredPages()
    {
        bool hasFiveHundredPages = _books
            .Any(b => b.PageCount > 500);
        
        Console.WriteLine(
            $"\nAre there any books over 500 pages long? {hasFiveHundredPages}"
        );
    }

    public void FirstOrDefaultById(int bookId)
    {
        var book = _books
            .FirstOrDefault(b => b.Id == bookId);
        
        Console.WriteLine(
            $"\nBook Id {bookId}: {book?.Title ?? "Not Found"}"
        );
    }

    public void JoinBooksAndAuthors()
    {
        var result = 
            _books.Join(
                _authors,
                b => b.AuthorId,
                a => a.Id,
                (book, author) => new
                {
                    book.Title,
                    AuthorName = author.Name
                }
            );
        
        Console.WriteLine("\nBooks and Authors:");

        foreach (var item in result)
            Console.WriteLine($"{item.Title} - {item.AuthorName}");
    }

    public void TopThreeLongestBooks()
    {
        var books = _books
            .OrderByDescending(b => b.PageCount)
            .Take(3);
        
        Console.WriteLine("\nTop 3 Longest Books:");

        foreach (var book in books)
            Console.WriteLine($"{book.Title} ({book.PageCount} pages)");
    }
}

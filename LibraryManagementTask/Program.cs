using LibraryManagementTask.Data;
using LibraryManagementTask.Models;
using LibraryManagementTask.Queries;
using LibraryManagementTask.Services;

List<Book> books = InMemoryStore.Books;
List<Author> authors = InMemoryStore.Authors;

IBookService bookService =
    new BookService(
        books, 
        authors
    );

var filters = 
    new LinqFilters(
        books,
        authors
    );

Console.WriteLine("\n=== Book List ===\n");

foreach (var book in bookService.GetAll())
{
    Console.WriteLine($"{book.Title} - {book.Author?.Name}:\nBook ID: {book.Id}\nAuthor ID: {book.AuthorId}\nPublication Date: {book.Year}\nPage Count: {book.PageCount}\n");
}

var newBook = bookService.Create(new Book
{
   Title = "The City and Its Uncertain Walls",
   Year = 2023,
   PageCount = 464,
   AuthorId = 2,
});

Console.WriteLine("Created Book:");
Console.WriteLine($"Title: {newBook.Title}\nBook ID: {newBook.Id}\nAuthor ID: {newBook.AuthorId}\nPublication Date: {newBook.Year}\nPage Count: {newBook.PageCount}\n");

var update = bookService.Update(new Book
{
   Id = newBook.Id,
   Title = "The City and Its Uncertain Walls (Large Print Edition)",
   Year = newBook.Year,
   PageCount = newBook.PageCount,
   AuthorId = newBook.AuthorId 
});

Console.WriteLine($"Update Successful? {update}\n");

var updatedBook = bookService.GetById(newBook.Id);

Console.WriteLine("Updated Book:");

if (updatedBook is not null)
{
    Console.WriteLine(
        $"{updatedBook.Id} - " +
        $"{updatedBook.Title} - " +
        $"{updatedBook.Author?.Name} - " +
        $"{updatedBook.Year} - " +
        $"{updatedBook.PageCount} pages\n"
    );
}

Console.WriteLine("=== Updated Book List ===\n");

foreach (var book in bookService.GetAll())
{
    Console.WriteLine($"{book.Title} - {book.Author?.Name}:\nBook ID: {book.Id}\nAuthor ID: {book.AuthorId}\nPublication Date: {book.Year}\nPage Count: {book.PageCount}\n");
}

var deleted = bookService.Delete(newBook.Id);

Console.WriteLine($"Delete Successful? {deleted}\n");

var deletedBook = bookService.GetById(newBook.Id);

Console.WriteLine("Verify Delete:\n");

if (deletedBook is null)
{
    Console.WriteLine("Book not found.\n");
}
else
{
    Console.WriteLine(deletedBook.Title);
}

Console.WriteLine("=== Updated Book List ===\n");

foreach (var book in bookService.GetAll())
{
    Console.WriteLine($"{book.Title} - {book.Author?.Name}:\nBook ID: {book.Id}\nAuthor ID: {book.AuthorId}\nPublication Date: {book.Year}\nPage Count: {book.PageCount}\n");
}

Console.WriteLine("=== LINQ Queries ===");

filters.FilterByAuthor("J.R.R Tolkien");
filters.SortTitles();
filters.GroupByAuthor();
filters.AveragePages();
filters.AnyOverFiveHundredPages();
filters.FirstOrDefaultById(4);
filters.JoinBooksAndAuthors();
filters.TopThreeLongestBooks();

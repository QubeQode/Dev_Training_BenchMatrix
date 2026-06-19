using LibraryManagementAPI.Models;

namespace LibraryManagementAPI.Data;

public static class InMemoryStore
{
    public static List<Author> Authors { get; } =
    [
        new Author { Id = 1, Name = "J.R.R Tolkien" },
        new Author { Id = 2, Name = "Haruki Murakami" },
        new Author { Id = 3, Name = "Elizabeth Moon" }
    ];

    public static List<Book> Books { get; } =
    [
        new Book { Id = 1, Title = "The Fellowship of the Ring", Year = 1954, PageCount = 423, AuthorId = 1 },
        new Book { Id = 2, Title = "The Two Towers", Year = 1954, PageCount = 352, AuthorId = 1 },
        new Book { Id = 3, Title = "The Return of the King", Year = 1956, PageCount = 416, AuthorId = 1 },
        new Book { Id = 4, Title = "Kafka on the Shore", Year = 2002, PageCount = 480, AuthorId = 2 },
        new Book { Id = 5, Title = "Killing Commendatore", Year = 2017, PageCount = 681, AuthorId = 2 },
        new Book { Id = 6, Title = "Sheepfarmer's Daughter", Year = 1988, PageCount = 416, AuthorId = 3 },
        new Book { Id = 7, Title = "Divided Allegiance", Year = 1988, PageCount = 480, AuthorId = 3 },
        new Book { Id = 8, Title = "Oath of Gold", Year = 1989, PageCount = 480, AuthorId = 3 }
    ];

    public static List<Tag> Tags { get; } =
    [
        new Tag { Id = 1, Name = "Fantasy" },
        new Tag { Id = 2, Name = "Surrealism" },
        new Tag { Id = 3, Name = "Classic" },
        new Tag { Id = 4, Name = "Postmodern" }
    ];

    public static List<BookTag> BookTags { get; } =
    [
        new BookTag { BookId = 1, TagId = 1 },
        new BookTag { BookId = 1, TagId = 3 },
        new BookTag { BookId = 4, TagId = 2 },
        new BookTag { BookId = 4, TagId = 4 },
        new BookTag { BookId = 6, TagId = 1 },
        new BookTag { BookId = 7, TagId = 1 }
    ];

    static InMemoryStore()
    {
        foreach (var book in Books)
        {
            var author = Authors.First(a => a.Id == book.AuthorId);
            book.Author = author;
            author.Books.Add(book);
        }
    }
}

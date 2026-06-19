using LibraryManagementTask.Models;

namespace LibraryManagementTask.Services;

public class BookService : IBookService
{
    private readonly List<Book> _books;
    private readonly List<Author> _authors;

    public BookService(List<Book> books, List<Author> authors)
    {
        _books = books;
        _authors = authors;
    }

    public IEnumerable<Book> GetAll()
    {
        return _books;
    }

    public Book? GetById(int id)
    {
        return _books.FirstOrDefault(b => b.Id == id);
    }

    public Book Create(Book book)
    {
        book.Id = _books.Max(b => b.Id) + 1;

        _books.Add(book);

        return book;
    }

    public bool Update(Book updatedBook)
    {
        var existingBook = GetById(updatedBook.Id);

        if (existingBook is null)
            return false;
        
        existingBook.Title = updatedBook.Title;
        existingBook.Year = updatedBook.Year;
        existingBook.PageCount = updatedBook.PageCount;
        existingBook.AuthorId = updatedBook.AuthorId;
        existingBook.Author = _authors.FirstOrDefault(a => a.Id == updatedBook.AuthorId);

        return true;
    }

    public bool Delete(int Id)
    {
        var book = GetById(Id);

        if (book is null)
            return false;
        
        _books.Remove(book);

        return true;
    }
}

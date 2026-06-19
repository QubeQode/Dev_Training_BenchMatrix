using LibraryManagementAPI.Exceptions;
using LibraryManagementAPI.Models;

namespace LibraryManagementAPI.Services;

public class BookService : IBookService
{
    private readonly List<Book> _books;
    private readonly List<Author> _authors;

    public BookService(
        List<Book> books,
        List<Author> authors)
    {
        _books = books;
        _authors = authors;
    }

    public Task<IEnumerable<Book>> GetAllAsync()
    {
        return Task.FromResult(_books.AsEnumerable());
    }

    public Task<Book> GetByIdAsync(int id)
    {
        var book = _books.FirstOrDefault(b => b.Id == id);

        if (book is null)
            throw new BookNotFoundException(id);
        
        return Task.FromResult(book);
    }

    public Task<Book> CreateAsync(Book book)
    {
        book.Id = _books.Max(b => b.Id) + 1;
        
        _books.Add(book);

        return Task.FromResult(book);
    }

    public async Task<bool> UpdateAsync(Book updatedBook)
    {
        var existingBook = 
            await GetByIdAsync(updatedBook.Id);
        
        existingBook.Title = updatedBook.Title;
        existingBook.Year = updatedBook.Year;
        existingBook.PageCount = updatedBook.PageCount;
        existingBook.AuthorId = updatedBook.AuthorId;
        existingBook.Author = 
            _authors.FirstOrDefault(
                a => a.Id == updatedBook.AuthorId
            );
        
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var book = await GetByIdAsync(id);

        _books.Remove(book);

        return true;
    }
}

/*
    - BookService should be a translation layer between Controller and data storage
    - Flow != Controller --> Book --> BookService
    - Flow = Controller --> BookCreate/Update/ResponseDTO --> BookService --> BookMapper --> BookEntity
    - In other words Service takes function of DTO <--> Entity translation
    - Because Controller doesn't need to know values or how an entity is constructed just needs to
    call in a service
    - Service in turn is what handles the details as a separation of concerns
*/

using LibraryManagementAPI.DTOs;
using LibraryManagementAPI.Exceptions;
using LibraryManagementAPI.Mappers;
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

    // Async procedure that takes in author?, page number, page size and returns collection of
    // BookResponse DTOs
    public Task<IEnumerable<BookResponseDTO>> GetAllAsync
    (
        string? author,
        int page,
        int pageSize
    )
    {
        // AsEnumerable converts book list to a queryable sequence of all books
        // Enumerable = read only and loopable - used for Skip() and Take() - also for Where() and Select()
        var query = _books.AsEnumerable();

        // Check if there is an optional parameter in the route for additional filtering and skip if absent
        if (!string.IsNullOrWhiteSpace(author))
        {
            query = query.Where(
                // Check that there is an author name for each book && name == optional param (ignore case)
                b => b.Author != null &&
                    b.Author.Name.Contains(
                        author,
                        StringComparison.OrdinalIgnoreCase
                    )
            );
        }

        // Pagination block - take filtered list and segment into chunks for display
        query = query
            // Define number of items to skip by working out previous page and items already processed
            .Skip((page - 1) * pageSize)
            // Display the correct chunk of data of the right size on the current page
            .Take(pageSize);
        
        // Convert the Enumerable of books into a enumerable of ResponseDTOs for the controller
        // Select() is taking each book --> transforming to DTO --> returning new collection
        var results = query.Select(BookMapper.ToResponse);

        return Task.FromResult(results);
    }

    public Task<BookResponseDTO> GetByIdAsync(int id)
    {
        // Find the entity we're looking for by first match or Null
        var book = _books.FirstOrDefault(b => b.Id == id);

        // If null throw an error through the error handler structure (BookNotFoundException.cs)
        if (book is null)
            throw new BookNotFoundException(id);

        // If book is found map entity to ResponseDTO and send to controller
        return Task.FromResult(
            BookMapper.ToResponse(book)
        );
    }

    public Task<BookResponseDTO> CreateAsync(BookCreateDTO dto)
    {
        var book = BookMapper.ToEntity(dto);

        // New Id assignment achieved on the server side
        book.Id = _books.Max(b => b.Id) + 1; 
        // Adding author by authorId since that isn't included in the DTO --> Entity mapping method
        book.Author = _authors.FirstOrDefault(a => a.Id == book.AuthorId);
        
        // Fleshed out entity is added to the memory store of books
        _books.Add(book);

        // Fleshed out entity is converted back to DTO to be used in response by Controller
        return Task.FromResult(BookMapper.ToResponse(book));
    }

    public async Task<BookResponseDTO> UpdateAsync(int id, BookUpdateDTO dto)
    {
        // Find entity we're looking for by first match or Null
        var existingBook = _books.FirstOrDefault(b => b.Id == id);

        // If null throw an error through the error handler structure (BookNotFoundException.cs)
        if (existingBook is null) throw new BookNotFoundException(id);
        
        // Reassign entity values to match the DTO values using Mapper method
        BookMapper.ApplyUpdate(existingBook, dto);
        
        // Convert updates entity to DTO to be used in response by Controller
        return BookMapper.ToResponse(existingBook);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        // Find entity we're looking for by first match or Null
        var book = _books.FirstOrDefault(b => b.Id == id);

        // If null throw an error through the error handler structure (BookNotFoundException.cs)
        if (book is null) throw new BookNotFoundException(id);

        // Remove entity from storage
        _books.Remove(book);

        return true;
    }
}

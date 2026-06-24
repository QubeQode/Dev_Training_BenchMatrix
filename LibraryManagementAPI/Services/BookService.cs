/*
    - BookService should be a translation layer between Controller and data storage
    - Flow != Controller --> Book --> BookService
    - Flow = Controller --> BookCreate/Update/ResponseDTO --> BookService --> BookMapper --> BookEntity
    - In other words Service takes function of DTO <--> Entity translation
    - Because Controller doesn't need to know values or how an entity is constructed just needs to
    call in a service
    - Service in turn is what handles the details as a separation of concerns
*/

using LibraryManagementAPI.Data;
using LibraryManagementAPI.DTOs;
using LibraryManagementAPI.Exceptions;
using LibraryManagementAPI.Mappers;
using LibraryManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementAPI.Services;

public class BookService : IBookService
{
    private readonly AppDbContext _context;

    public BookService(AppDbContext context)
    {
        _context = context;
    }

    // Async procedure that takes in author?, page number, page size and returns collection of
    // BookResponse DTOs
    public async Task<IEnumerable<BookResponseDTO>> GetAllAsync
    (
        string? author,
        int page,
        int pageSize
    )
    {
        var query = _context.Books // Books now comes from Db
            .Include(b => b.Author) // Load nav property to authors
            .AsNoTracking(); // Read only query

        // Check if there is an optional parameter in the route for additional filtering and skip if absent
        if (!string.IsNullOrWhiteSpace(author))
        {
            query = query.Where(
                // Check that there is an author name for each book && name == optional param (ignore case)
                b => b.Author != null &&
                    b.Author.Name.Contains(
                        author
                        // StringComparison.OrdinalIgnoreCase - No longer necessary
                        // EF core uses LIKE '%authorName%' pattern implicitly
                    )
            );
        }

        // Pagination block - take filtered list and segment into chunks for display
        var books = await query
            // Define number of items to skip by working out previous page and items already processed
            .Skip((page - 1) * pageSize)
            // Display the correct chunk of data of the right size on the current page
            .Take(pageSize)
            .ToListAsync();
        
        // Select() is taking each book --> transforming to DTO --> returning new collection
        return books.Select(BookMapper.ToResponse);
    }

    public async Task<BookResponseDTO> GetByIdAsync(int id)
    {
        // Find the entity we're looking for by first match or Null
        var book = await _context.Books // Books now comes from Db
            .Include(b => b.Author) // Load nav property to authors
            .AsNoTracking() // Read only query
            .FirstOrDefaultAsync(b => b.Id == id);

        // If null throw an error through the error handler structure (BookNotFoundException.cs)
        if (book is null) throw new BookNotFoundException(id);

        // If book is found map entity to ResponseDTO and send to controller
        return BookMapper.ToResponse(book);
    }

    public async Task<BookResponseDTO> CreateAsync(BookCreateDTO dto)
    {
        var book = BookMapper.ToEntity(dto);

        /*
            Following block no longer necessary as EF core implicitly understands
            auto-incremental ID assignment + author-book relationship:
                // New Id assignment achieved on the server side
                book.Id = _books.Max(b => b.Id) + 1; 
                // Adding author by authorId since that isn't included in the DTO --> Entity mapping method
                book.Author = _authors.FirstOrDefault(a => a.Id == book.AuthorId);
            Can simply add the new Entity to the Db
        */
        
        // Mark entity for addition to the memory store of books
        _context.Books.Add(book);

        // Staged changes are executed
        await _context.SaveChangesAsync();

        // Fleshed out entity is converted back to DTO to be used in response by Controller
        return BookMapper.ToResponse(book);
    }

    public async Task<BookResponseDTO> UpdateAsync(int id, BookUpdateDTO dto)
    {
        // Find entity we're looking for by first match or Null
        var existingBook = await _context.Books
            .FirstOrDefaultAsync(b => b.Id == id);

        // If null throw an error through the error handler structure (BookNotFoundException.cs)
        if (existingBook is null) throw new BookNotFoundException(id);
        
        // Mark entity for value reassignment to match the DTO values using Mapper method
        BookMapper.ApplyUpdate(existingBook, dto);

        // Staged changes are executed
        await _context.SaveChangesAsync();
        
        // Convert updates entity to DTO to be used in response by Controller
        return BookMapper.ToResponse(existingBook);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        // Find entity we're looking for by first match or Null
        var book = await _context.Books
            .FirstOrDefaultAsync(b => b.Id == id);

        // If null throw an error through the error handler structure (BookNotFoundException.cs)
        if (book is null) throw new BookNotFoundException(id);

        // Mark entity for removal from storage
        _context.Books.Remove(book);

        // Staged changes are executed
        await _context.SaveChangesAsync();

        return true;
    }
}

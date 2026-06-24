/*
    - Mapper layer functions as a covnersion layer between DTO <--> Entities (models)
    - Ideal Create Flow: BookCreateDTO --> BookMapper.ToEntity() --> Book Entity --> BookService
    - Ideal Response Flow: Book Entity --> BookMapper.ToResponse() --> BookResponseDTO --> Api Response
    - Purpose:
        - Separation of concerns
        - Safer API
        - Less exposure of Db fields
        - Easier to edit and update methods and server processes etc
    - Methods:
        - ToEntity() - BookCreateDTO --> Book (intakes client side JSON for a new book)
        - ApplyUpdate() - Db Book --> Apply Update --> Modified Book
        - ToResponse() - Book --> BookResponseDTO
*/

using LibraryManagementAPI.Models;
using LibraryManagementAPI.DTOs;

namespace LibraryManagementAPI.Mappers;

public static class BookMapper
{
    // No I/O happening here, so no async/await - everything is happening in the CPU itself
    public static Book ToEntity(BookCreateDTO dto)
    {
        return new Book
        {
          Title = dto.Title,
          Year = dto.Year,
          PageCount = dto.PageCount,
          Isbn = dto.Isbn,
          AuthorId = dto.AuthorId  
        };
    }

    public static void ApplyUpdate(Book book, BookUpdateDTO dto)
    {
        book.Title = dto.Title;
        book.Year = dto.Year;
        book.PageCount = dto.PageCount;
        book.Isbn = dto.Isbn;
        book.AuthorId = dto.AuthorId;
    }

    public static BookResponseDTO ToResponse(Book book)
    {
        return new BookResponseDTO
        {
            Id = book.Id,
            Title = book.Title,
            Year = book.Year,
            PageCount = book.PageCount,
            Isbn = book.Isbn,
            AuthorId = book.AuthorId
        };
    }
}

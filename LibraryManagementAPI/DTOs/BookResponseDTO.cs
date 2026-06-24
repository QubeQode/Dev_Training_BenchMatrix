/*
    - BookResponseDTO only contains fields that are used to define what the API sends back to the client
    - Safe, controlled representation of a book
    - Use record instead of class because this is a data container and we don't want data mutability to be
    possible through a response
    - Use init so values can be defined in instantiation but never changed after the fact
*/

namespace LibraryManagementAPI.DTOs;

public record BookResponseDTO
{
    // Id is introduced here because the client needs to know how to access the information via a reference
    public int Id { get; init; }

    public string Title { get; init; } = string.Empty;

    public int Year { get; init; }

    public int PageCount { get; init; }

    public string? Isbn { get; set; }

    public int AuthorId { get; init; }
}

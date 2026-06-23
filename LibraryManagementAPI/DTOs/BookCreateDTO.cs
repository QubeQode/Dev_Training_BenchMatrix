/*
    - BookCreateDTO only contains creatable fields
    - Ensures validation through the [Required], [StringLength] and [Range] attributes
    - [ApiController] attribute automates request code + error data when these attributes fail validation check
    - Contains only the bare minimum information needed for creating a new book entry
*/

using System.ComponentModel.DataAnnotations;

namespace LibraryManagementAPI.DTOs;

public class BookCreateDTO
{
    // There is no Id because this is all done on the server side logic and when interfacing with db
    // Isn't needed in DTO layer which handles the shaping and validation of info recieved by Controller

    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [Range(1000, 3000)] // Between the years 1XXX - 2XXX
    public int Year { get; set; }

    [Range(1, 10000)] // Disallow a 0 page book
    public int PageCount { get; set; }

    [Required]
    public int AuthorId { get; set; }
}

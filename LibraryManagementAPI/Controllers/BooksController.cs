using LibraryManagementAPI.DTOs;
using LibraryManagementAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")] // useRouting() used for this routing functionality
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(
        IBookService bookService
    )
    {
        _bookService = bookService;
    }

    [Authorize]
    [HttpGet] // <-- This is the routing layer
    public async Task<ActionResult<IEnumerable<BookResponseDTO>>> GetAll
    (
        string? author,
        int page = 1, // Default values allow for uri without specified values to still work
        int pagesize = 10 // Default values allow for uri without specified values to still work
    )
    {
        var books = await _bookService.GetAllAsync(
            author,
            page,
            pagesize
        );

        return Ok(books);
    }

    [Authorize]
    [HttpGet("{id}")] // <-- Autobinding on the ID portion of the uri
    public async Task <ActionResult<BookResponseDTO>> GetById(int id)
    {
        var book = await _bookService.GetByIdAsync(id);

        return Ok(book);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<BookResponseDTO>> Create(BookCreateDTO dto)
    {
        var createdBook = await _bookService.CreateAsync(dto);

        // CreatedAtAction generates uri source from action name and route values
        // Effectively a route template of api/books/{id}
        return CreatedAtAction(
            nameof(GetById), // Directs ASP.NET to GetById method
            new {id = createdBook.Id}, // Identified Id to plug into GetById method
            createdBook // Response body of the actual action performed
        );
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<ActionResult<BookResponseDTO>> Update(int id, BookUpdateDTO dto)
    {
        var updatedBook = await _bookService.UpdateAsync(id, dto);

        return Ok(updatedBook);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _bookService.DeleteAsync(id);

        return NoContent();
    }
}

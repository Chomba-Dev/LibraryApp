using Microsoft.AspNetCore.Mvc;
using App.Shared.Models;
using App.Shared.Responses;
using App.Shared.Services;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }
    
    //GET: api/books
    [HttpGet("GetBooks")]
    public async Task<ActionResult<ServiceResponse<List<Book>>>> GetBooks()
    {
        var response = await _bookService.GetBooksAsync();
        return Ok(response);
    }
    
    //GET: api/books/{id}
    [HttpGet("GetBook/{id}")]
    public async Task<ActionResult<ServiceResponse<Book>>> GetBook(Guid id)
    {
        var response = await _bookService.GetBookAsync(id);
        if (!response.Success)
        {
            return NotFound(response);
        }
        return Ok(response);
    }
    
    //POST: api/books
    [Authorize(Roles = "Admin, Librarian")]
    [HttpPost("AddBook")]
    public async Task<ActionResult<ServiceResponse<Book>>> CreateBook([FromBody] Book book)
    {
        var response = await _bookService.CreateBookAsync(book);
        return CreatedAtAction(nameof(GetBook), new { id = book.Id }, response);
    }
    
    //PUT: api/books/{id}
    [Authorize(Roles = "Admin, Librarian")]
    [HttpPut("Update/{id}")]
    public async Task<ActionResult<ServiceResponse<Book>>> UpdateBook(Guid id, [FromBody] Book book)
    {
        if (id != book.Id)
        {
            return BadRequest(new ServiceResponse<Book>
            {
                Success = false,
                Message = "Ids don't match"
            });
        }
        
        var response = await _bookService.UpdateBookAsync(id, book);
        if (!response.Success)
        {
            return BadRequest(response);
        }
        return Ok(response);
    }
    
    //DELETE: api/books/{id}
    [Authorize(Roles = "Admin, Librarian")]
    [HttpDelete("Delete/{id}")]
    public async Task<ActionResult<ServiceResponse<Book>>> DeleteBook(Guid id)
    {
        var response = await _bookService.DeleteBookAsync(id);
        if (!response.Success)
        {
            return NotFound(response);
        }
        return Ok(response);
    }
}
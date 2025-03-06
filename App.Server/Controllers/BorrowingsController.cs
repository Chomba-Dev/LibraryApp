using Microsoft.AspNetCore.Mvc;
using App.Shared.Models;
using App.Shared.Responses;
using App.Shared.Services;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]")]
public class BorrowingsController : ControllerBase
{
    private readonly IBorrowingService _borrowingService;

    public BorrowingsController(IBorrowingService borrowingService)
    {
        _borrowingService = borrowingService;
    }
    
    //GET: api/borrowings
    [Authorize(Roles = "Admin, Librarian")]
    [HttpGet("show-borrowings")]
    public async Task<ActionResult<ServiceResponse<List<Borrowing>>>> GetBorrowingsAsync()
    {
        var response = await _borrowingService.GetBorrowingsAsync();
        return Ok(response);
    }
    
    //POST: api/borrowings/borrow
    [Authorize(Roles = "Admin, Librarian, Member")]
    [HttpPost("borrow")]
    public async Task<ActionResult<ServiceResponse<Borrowing>>> BorrowBook([FromBody] BorrowRequest request)
    {
        var response = await _borrowingService.BorrowBookAsync(request.BookId, request.UserId);
        if (!response.Success)
        {
            return BadRequest(response);
        }
        return Ok(response);
    }
    
    //PUT: api/borrowings/return/{id}
    [Authorize(Roles = "Admin, Librarian, Member")]
    [HttpPut("return-book/{id:guid}")]
    public async Task<ActionResult<ServiceResponse<Borrowing>>> ReturnBook(Guid id)
    {
        var response = await _borrowingService.ReturnBookAsync(id);
        if (!response.Success)
        {
            return BadRequest(response);
        }
        return Ok(response);
    }
    
    //GET: api/borrowings/user/{userId}
    [Authorize(Roles = "Admin, Librarian, Member")]
    [HttpGet("show-borrowings-by-Id/{userId}")]
    public async Task<ActionResult<ServiceResponse<List<Borrowing>>>> GetBorrowingsByUserId(String userId)
    {
        var response = await _borrowingService.GetBorrowingsByUserIdAsync(userId);
        return Ok(response);
    }
}
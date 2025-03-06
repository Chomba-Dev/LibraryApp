using App.Shared.Models;
using App.Shared.Responses;
using System;

namespace App.Shared.Services
{
    public interface IBorrowingService
    {
        Task<ServiceResponse<List<Borrowing>>> GetBorrowingsAsync();
        Task<ServiceResponse<Borrowing>> BorrowBookAsync(Guid bookId, String userId);
        Task<ServiceResponse<Borrowing>> ReturnBookAsync(Guid borrowingId);
        Task<ServiceResponse<List<Borrowing>>> GetBorrowingsByUserIdAsync(String userId);
    }
}


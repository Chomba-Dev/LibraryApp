using Microsoft.EntityFrameworkCore;
using App.Shared.Models;
using App.Shared.Responses;
using App.Shared.Services;
using App.Server.Data;

namespace App.Server.Services
{
    public class BorrowingService : IBorrowingService
    {
        private readonly ApplicationDbContext _context;

        public BorrowingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<List<Borrowing>>> GetBorrowingsAsync()
        {
            var borrowings = await _context.Borrowings
                .Include(b => b.Book)
                .Include(b => b.User)
                .ToListAsync();

            return new ServiceResponse<List<Borrowing>>
            {
                Success = true,
                Message = "Borrowings retrieved successfully",
                Data = borrowings
            };
        }

        public async Task<ServiceResponse<Borrowing>> BorrowBookAsync(Guid bookId, String userId)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book == null)
            {
                return new ServiceResponse<Borrowing>
                {
                    Success = false,
                    Message = "Book not found"
                };
            }
            
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return new ServiceResponse<Borrowing>
                {
                    Success = false,
                    Message = "User not found"
                };
            }

            if (book.AvailableCopies <= 0)
            {
                return new ServiceResponse<Borrowing>
                {
                    Success = false,
                    Message = "No available copies of this book"
                };
            }

            var borrowing = new Borrowing
            {
                BookId = bookId,
                UserId = userId,
                BorrowDate = DateTime.UtcNow,
                ReturnDate = null
            };
            
            book.AvailableCopies--;
            
            _context.Borrowings.Add(borrowing);
            await _context.SaveChangesAsync();

            return new ServiceResponse<Borrowing>
            {
                Success = true,
                Message = "Borrowing successfully",
                Data = borrowing
            };
        }

        public async Task<ServiceResponse<Borrowing>> ReturnBookAsync(Guid borrowingId)
        {
            var borrowing = await _context.Borrowings
                .Include(b => b.Book)
                .FirstOrDefaultAsync(b => b.Id == borrowingId);

            if (borrowing == null)
            {
                return new ServiceResponse<Borrowing>
                {
                    Success = false,
                    Message = "Borrowing not found"
                };
            }

            if (borrowing.ReturnDate.HasValue)
            {
                return new ServiceResponse<Borrowing>
                {
                    Success = false,
                    Message = "Book already returned"
                };
            }
            
            borrowing.ReturnDate = DateTime.UtcNow;
            
            borrowing.Book.AvailableCopies++;
            
            await _context.SaveChangesAsync();
            
            return new ServiceResponse<Borrowing>
            {
                Success = true,
                Message = "Book returned successfully",
                Data = borrowing
            };
        }

        public async Task<ServiceResponse<List<Borrowing>>> GetBorrowingsByUserIdAsync(String userId)
        {
            var borrowings = await _context.Borrowings
                .Include(b => b.Book)
                .Where(b => b.UserId == userId)
                .ToListAsync();

            return new ServiceResponse<List<Borrowing>>
            {
                Success = true,
                Message = "Borrowings retrieved successfully",
                Data = borrowings
            };
        }
    }
}


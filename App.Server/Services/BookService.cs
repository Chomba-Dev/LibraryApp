using Microsoft.EntityFrameworkCore;
using App.Shared.Models;
using App.Shared.Responses;
using App.Shared.Services;
using App.Server.Data;


namespace App.Server.Services
{
    public class BookService : IBookService
    {
        private readonly ApplicationDbContext _context;

        public BookService(ApplicationDbContext context)
        {
            _context = context;
        }
        
        //Get all books
        public async Task<ServiceResponse<List<Book>>> GetBooksAsync()
        {
            var books = await _context.Books.ToListAsync();
            return new ServiceResponse<List<Book>>
            {
                Success = true,
                Message = "Books retrieved successfully",
                Data = books
            };
        }
        
        //Get a single book by ID
        public async Task<ServiceResponse<Book>> GetBookAsync(Guid id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return new ServiceResponse<Book>
                {
                    Success = false,
                    Message = "Book not found"
                };
            }

            return new ServiceResponse<Book>
            {
                Success = true,
                Message = "Book retrieved successfully",
                Data = book
            };
        }
        
        //Create a new book
        public async Task<ServiceResponse<Book>> CreateBookAsync(Book book)
        {
            book.Id = Guid.NewGuid();
            
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return new ServiceResponse<Book>
            {
                Success = true,
                Message = "Book created successfully",
                Data = book
            };
        }
        
        //Update an existing book
        public async Task<ServiceResponse<Book>> UpdateBookAsync(Guid id, Book book)
        {
            if (id != book.Id)
            {
                return new ServiceResponse<Book>
                {
                    Success = false,
                    Message = "Book ID mismatch"
                };
            }
            
            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Books.Any(e => e.Id == book.Id))
                {
                    return new ServiceResponse<Book>
                    {
                        Success = false,
                        Message = "Book not found"
                    };
                }
                else
                {
                    throw;
                }
            }

            return new ServiceResponse<Book>
            {
                Success = true,
                Message = "Book updated successfully",
                Data = book
            };
        }
        
        //Delete a book
        public async Task<ServiceResponse<bool>> DeleteBookAsync(Guid id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return new ServiceResponse<bool>
                {
                    Success = false,
                    Message = "Book not found"
                };
            }
            
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return new ServiceResponse<bool>
            {
                Success = true,
                Message = "Book deleted successfully",
                Data = true
            };
        }
    }
}


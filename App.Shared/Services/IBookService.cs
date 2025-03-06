using App.Shared.Models;
using App.Shared.Responses;

namespace App.Shared.Services
{
    public interface IBookService
    {
        Task<ServiceResponse<List<Book>>> GetBooksAsync();
        Task<ServiceResponse<Book>> GetBookAsync(Guid id);
        Task<ServiceResponse<Book>> CreateBookAsync(Book book);
        Task<ServiceResponse<Book>> UpdateBookAsync(Guid id, Book book);
        Task<ServiceResponse<bool>> DeleteBookAsync(Guid id);
    }
}
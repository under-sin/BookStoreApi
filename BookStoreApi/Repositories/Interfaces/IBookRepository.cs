using BookStoreApi.Models;

namespace BookStoreApi.Repositories.Interfaces;

public interface IBookRepository
{
    // getAllAsync
    Task<IEnumerable<Book>> GetAllAsync();
    // getByIdAsync
    Task<Book> GetByIdAsync(string id);
    // createBookAsync
    Task CreateBookAsync(Book book);
    // updateBookAsync
    Task UpdateBookAsync(Book book);
    // removeBookAsync
    Task RemoveBookAsync(string id);
}

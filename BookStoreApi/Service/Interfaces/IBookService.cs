using BookStoreApi.ViewModel;

namespace BookStoreApi;

public interface IBookService
{
    // getAllAsync
    Task<IEnumerable<BookViewModel>> GetAllAsync();
    // getByIdAsync
    Task<BookViewModel> GetByIdAsync(string id);
    // createBookAsync
    Task<string> CreateBookAsync(BookViewModel book);
    // updateBookAsync
    Task UpdateBookAsync(BookViewModel book);
    // removeBookAsync
    Task RemoveBookAsync(string id);
}

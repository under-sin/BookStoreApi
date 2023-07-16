using AutoMapper;
using BookStoreApi.Models;
using BookStoreApi.Repositories.Interfaces;
using BookStoreApi.ViewModel;

namespace BookStoreApi.Service;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;
    private readonly IMapper _mapper;

    public BookService(
        IBookRepository bookRepository,
        IMapper mapper)
    {
        _bookRepository = bookRepository;
        _mapper = mapper;
    }

    public async Task<List<BookViewModel>> GetPagedAsync(int pageNumber, int pageSize)
    {
        var pagedBooks = await _bookRepository.GetPagedAsync(pageNumber, pageSize);
        var booksViewModel = _mapper.Map<List<BookViewModel>>(pagedBooks);

        return booksViewModel;
    }

    public async Task<IEnumerable<BookViewModel>> GetAllAsync()
    {
        var books = await _bookRepository.GetAllAsync();
        var booksViewModel = _mapper.Map<IEnumerable<BookViewModel>>(books);

        return booksViewModel;
    }

    public async Task<BookViewModel> GetByIdAsync(string id)
    {
        var book = await _bookRepository.GetByIdAsync(id);

        return _mapper.Map<BookViewModel>(book);
    }

    public async Task<string> CreateBookAsync(BookViewModel bookViewModel)
    {
        var book = _mapper.Map<Book>(bookViewModel);
        await _bookRepository.CreateBookAsync(book);

        return book.Id!;
    }

    public async Task UpdateBookAsync(BookViewModel bookViewModel)
    {
        var book = _mapper.Map<Book>(bookViewModel);
        await _bookRepository.UpdateBookAsync(book);
    }

    public async Task RemoveBookAsync(string id)
        => await _bookRepository.RemoveBookAsync(id);

}

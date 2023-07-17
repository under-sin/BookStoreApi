using AutoMapper;
using BookStoreApi.Models;
using BookStoreApi.Repositories.Interfaces;
using BookStoreApi.ViewModel;
using Microsoft.Extensions.Caching.Memory;

namespace BookStoreApi.Service;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;
    private readonly IMapper _mapper;
    private readonly IMemoryCache _cache;

    public BookService(
        IBookRepository bookRepository,
        IMapper mapper,
        IMemoryCache cache)
    {
        _bookRepository = bookRepository;
        _mapper = mapper;
        _cache = cache;
    }

    public async Task<List<BookViewModel>> GetPagedAsync(int pageNumber, int pageSize)
    {
        // verifica se os dados estão em cache
        if(_cache.TryGetValue<List<BookViewModel>>("BooksViewModelCache", out var cachedBooksViewModel))
            return cachedBooksViewModel!;

        var pagedBooks = await _bookRepository.GetAllAsync();
        var booksViewModel = _mapper.Map<List<BookViewModel>>(pagedBooks);

        // defini a duração do cache, quanto tempo ele vai durar.
        var cacheOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(10));

        // define o cache
        _cache.Set("BooksViewModelCache", booksViewModel, cacheOptions);

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

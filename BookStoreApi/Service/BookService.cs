using BookStoreApi.Models;
using BookStoreApi.Repositories.Interfaces;
using BookStoreApi.Settings;
using BookStoreApi.ViewModel;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BookStoreApi.Service;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;

    public BookService(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<IEnumerable<BookViewModel>> GetAllAsync()
    {
        var books = await _bookRepository.GetAllAsync();

        var booksViewModel = books.Select(book => new BookViewModel
        {
            Id = book.Id,
            BookName = book.BookName,
            Category = book.Category,
            Price = book.Price,
            Author = book.Author
        });

        return booksViewModel;
    }

    public async Task<BookViewModel> GetByIdAsync(string id)
    {
        var book = await _bookRepository.GetByIdAsync(id);

        var bookViewModel = new BookViewModel
        {
            Id = book.Id,
            BookName = book.BookName,
            Category = book.Category,
            Price = book.Price,
            Author = book.Author
        };

        return bookViewModel;
    }

    public async Task<string> CreateBookAsync(BookViewModel bookViewModel)
    {
        Book book = new()
        {
            BookName = bookViewModel.BookName,
            Category = bookViewModel.Category,
            Price = bookViewModel.Price,
            Author = bookViewModel.Author
        };

        await _bookRepository.CreateBookAsync(book);
        return book.Id!;
    }

    public async Task UpdateBookAsync(BookViewModel bookViewModel)
    {
        Book book = new()
        {
            Id = bookViewModel.Id,
            BookName = bookViewModel.BookName,
            Category = bookViewModel.Category,
            Price = bookViewModel.Price,
            Author = bookViewModel.Author
        };

        await _bookRepository.UpdateBookAsync(book);
    }

    public async Task RemoveBookAsync(string id)
        => await _bookRepository.RemoveBookAsync(id);

}

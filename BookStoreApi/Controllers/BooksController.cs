using BookStoreApi.Models;
using BookStoreApi.Service;
using BookStoreApi.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApi.Controllers;

[ApiController]
[Route("books")]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService) => _bookService = bookService;

    [HttpGet]
    public async Task<IEnumerator<BookViewModel>> GetBooksAsync() 
        => (IEnumerator<BookViewModel>)await _bookService.GetAllAsync();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<BookViewModel>> GetBookAsync(string id)
    {
        var book = await _bookService.GetByIdAsync(id);

        return book is null ? NotFound() : book;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(BookViewModel newBook)
    {
        var newBookId = await _bookService.CreateBookAsync(newBook);
        newBook.Id = newBookId;

        return CreatedAtAction(nameof(GetBookAsync), new { id = newBookId }, newBook);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> UpdateAsync(string id, BookViewModel updateBook)
    {
        var book = await _bookService.GetByIdAsync(id);

        if(book is null)
            return NotFound();

        updateBook.Id = book.Id;

        await _bookService.UpdateBookAsync(updateBook);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> DeleteAsync(string id)
    {
        var book = await _bookService.GetByIdAsync(id);

        if(book is null)
            return NotFound();

        await _bookService.RemoveBookAsync(id);

        return NoContent();
    }
}

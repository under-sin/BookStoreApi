using BookStoreApi.Models;
using BookStoreApi.Service;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApi.Controllers;

[ApiController]
[Route("books")]
public class BooksController : ControllerBase
{
    private readonly BooksService _booksService;

    public BooksController(BooksService booksService) => _booksService = booksService;

    [HttpGet]
    public async Task<List<Book>> GetBooksAsync() => await _booksService.GetBooksAsync();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Book>> GetBookAsync(string id)
    {
        var book = await _booksService.GetBookAsync(id);

        return book is null ? NotFound() : book;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(Book newBook)
    {
        await _booksService.CreateAsync(newBook);

        return CreatedAtAction(nameof(GetBookAsync), new { id = newBook.Id }, newBook);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> UpdateAsync(string id, Book updateBook)
    {
        var book = await _booksService.GetBookAsync(id);

        if(book is null)
            return NotFound();

        updateBook.Id = book.Id;

        await _booksService.UpdateAsync(id, updateBook);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> DeleteAsync(string id)
    {
        var book = await _booksService.GetBookAsync(id);

        if(book is null)
            return NotFound();

        await _booksService.RemoveAsync(id);

        return NoContent();
    }
}

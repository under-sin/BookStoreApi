using System.Data.Common;
using BookStoreApi.Extensions;
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
    public async Task<ActionResult<List<BookViewModel>>> GetAllAsync(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        try
        {
            var pagedBooks = (await _bookService.GetPagedAsync(pageNumber, pageSize)).ToList();

            return Ok(new ResultViewModel<List<BookViewModel>>(pagedBooks));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<List<BookViewModel>>("Falha interna no servido"));
        }
    }

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<BookViewModel>> GetByIdAsync(
        [FromRoute] string id)
    {
        try
        {
            var book = await _bookService.GetByIdAsync(id);

            if (book is null)
                return NotFound(new ResultViewModel<BookViewModel>("Conteúdo não encontrado"));

            return Ok(new ResultViewModel<BookViewModel>(book));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<BookViewModel>("Falha interna no servido"));
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateBookAsync(
        [FromBody] BookViewModel newBook)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<BookViewModel>(ModelState.GetErrors()));

        try
        {
            var newBookId = await _bookService.CreateBookAsync(newBook);
            newBook.Id = newBookId;

            return CreatedAtAction(nameof(GetByIdAsync), new { id = newBookId }, new ResultViewModel<BookViewModel>(newBook));
        }
        catch (DbException)
        {
            return StatusCode(500, new ResultViewModel<BookViewModel>("Não foi possivel incluir o livro"));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<BookViewModel>("Falha interna no servido"));
        }

    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> UpdateBookAsync(
        [FromRoute] string id, 
        [FromBody] BookViewModel updateBook)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<BookViewModel>(ModelState.GetErrors()));

        try
        {
            var book = await _bookService.GetByIdAsync(id);

            if (book is null)
                return NotFound(new ResultViewModel<BookViewModel>("Conteúdo não encontrado"));

            updateBook.Id = book.Id;

            await _bookService.UpdateBookAsync(updateBook);

            return Ok(new ResultViewModel<BookViewModel>(book));
        }
        catch (DbException)
        {
            return StatusCode(500, new ResultViewModel<BookViewModel>("Não foi possivel atualizar o livro"));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<BookViewModel>("Falha interna no servido"));
        }

    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> RemoveBookAsync(
        [FromRoute] string id)
    {
        var book = await _bookService.GetByIdAsync(id);

        try
        {
            if (book is null)
                return NotFound(new ResultViewModel<BookViewModel>("Conteúdo não encontrado"));

            await _bookService.RemoveBookAsync(id);

            return Ok(new ResultViewModel<BookViewModel>(book));
        }
        catch (DbException)
        {
            return StatusCode(500, new ResultViewModel<BookViewModel>("Não foi possivel atualizar o livro"));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<BookViewModel>("Falha interna no servido"));
        }

    }
}

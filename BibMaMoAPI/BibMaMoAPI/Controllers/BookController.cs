using BibMaMo.Core.Entities;
using BibMaMo.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BibMaMo.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class BookController : ControllerBase
  {
    IBookRepository _BookRepository;
    public BookController(IBookRepository BookRepostitory)
    {
      _BookRepository = BookRepostitory;
    }
    [HttpGet]
    public async Task<IActionResult> GetBooks()
    {
      var Books = await _BookRepository.GetBooks();
      return Ok(Books);
    }
    [HttpGet("/{handle}")]
    public async Task<IActionResult> GetSingleBook(string handle)
    {
      return Ok(await _BookRepository.GetBook(handle));
    }
    [HttpDelete("/{handle}")]
    public async Task<IActionResult> DeleteBook(string handle)
    {
      await _BookRepository.DeleteBook(handle);
      return Ok(null);
    }
    [HttpPost]
    public async Task<IActionResult> InsertBook(Book Book)
    {
      Book.Handle = string.Empty;
      Book = await _BookRepository.AddBook(Book);
      return Ok(Book);
    }
    [HttpPut]
    public async Task<IActionResult> UpdateBook(Book Book)
    {
      Book = await _BookRepository.AddBook(Book);
      return Ok(Book);
    }

  }
}

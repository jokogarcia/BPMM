using BibMaMo.Core.Entities;
using BibMaMo.Core.Exceptions;
using BibMaMo.Core.Interfaces;
using BibMaMo.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BibMaMo.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class BookController : ControllerBase
  {
    IBookRepository _repository;
    public BookController(IBookRepository repository)
    {
      _repository = repository;
    }
    [HttpGet]
    public async Task<IActionResult> Get(string tags = "")
    {
      var Books = await _repository.Get(tags);
      return Ok(Books);
    }
    [HttpGet("{handle}")]
    public async Task<IActionResult> GetSingle(string handle)
    {
      try
      {
        return Ok(await _repository.GetSingle(handle));
      }
      catch (ItemNotFoundException)
      {
        return NotFound();
      }
    }
    [HttpDelete("{handle}")]
    public async Task<IActionResult> Remove(string handle)
    {
      try
      {
        await _repository.Remove(handle);
      }
      catch (ItemNotFoundException)
      {
        return NotFound();
      }
      return Ok();
    }
    [HttpPost]
    public async Task<IActionResult> Insert(Book Book)
    {
      Book.Handle = string.Empty;
      Book = await _repository.Insert(Book);
      return Ok(Book);
    }
    [HttpPut]
    public async Task<IActionResult> Replace(Book Book)
    {
      try
      {
        await _repository.Replace(Book);
        return Ok();
      }
      catch (ItemNotFoundException)
      {
        return NotFound();
      }

    }

  }
}

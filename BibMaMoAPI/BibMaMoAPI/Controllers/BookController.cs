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
    public BookController(IBookRepository articleRepostitory)
    {
      _repository = articleRepostitory;
    }
    [HttpGet]
    public async Task<IActionResult> Get()
    {
      var articles = await _repository.Get();
      return Ok(articles);
    }
    [HttpGet("filtered/{tags}")]
    public async Task<IActionResult> GetFiltered(string tags)
    {
      if (string.IsNullOrEmpty(tags))
      {
        return BadRequest();
      }
      try
      {
        var articles = await _repository.GetFiltered(tags);
        return Ok(articles);
      }
      catch (ItemNotFoundException)
      {
        return NotFound();
      }
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
    public async Task<IActionResult> Insert(Book article)
    {
      article.Handle = string.Empty;
      article = await _repository.Insert(article);
      return Ok(article);
    }
    [HttpPut]
    public async Task<IActionResult> Replace(Book article)
    {
      try
      {
        await _repository.Replace(article);
        return Ok();
      }
      catch (ItemNotFoundException)
      {
        return NotFound();
      }

    }

  }
}

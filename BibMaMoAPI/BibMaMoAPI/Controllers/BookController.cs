using BibMaMo.Core.Entities;
using BibMaMo.Core.Exceptions;
using BibMaMo.Core.Interfaces;
using BibMaMo.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Text;
using System.Globalization;

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
    public async Task<IActionResult> Get()
    {
      var books = await _repository.Get();
      return Ok(books);
    }
    [HttpGet("tags/{tags}")]
    public async Task<IActionResult> GetFiltered(string tags)
    {
      if (string.IsNullOrEmpty(tags))
      {
        return BadRequest();
      }
      try
      {
        var books = await _repository.GetFilteredWithTags(tags);
        return Ok(books);
      }
      catch (ItemNotFoundException)
      {
        return NotFound();
      }
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSingle(int id)
    {
      try
      {
        return Ok(await _repository.GetSingle(id));
      }
      catch (ItemNotFoundException)
      {
        return NotFound();
      }
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Remove(int id)
    {
      try
      {
        await _repository.Remove(id);
      }
      catch (ItemNotFoundException)
      {
        return NotFound();
      }
      return Ok();
    }
    [HttpPost]
    public async Task<IActionResult> Insert(Book item)
    {
      item.BookId = 0;
      item = await _repository.Insert(item);
      return Ok(item);
    }
    [HttpPut]
    public async Task<IActionResult> Replace(Book item)
    {
      try
      {
        await _repository.Replace(item);
        return Ok();
      }
      catch (ItemNotFoundException)
      {
        return NotFound();
      }

    }
    [HttpGet("filtered")]
    public async Task<IActionResult> Search(string author = "", string title = "", int pageSize = 25, int pageNumber = 0)
    {
      return Ok(await _repository.GetFiltered(author, title, pageSize, pageNumber));
    }
    [HttpGet("categories")]
    public Task<IEnumerable<string>> GetCategories()
    {
      return _repository.GetCategories();
    }


  }
}

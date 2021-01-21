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
  public class ArticleController : ControllerBase
  {
    IArticleRepository _repository;
    public ArticleController(IArticleRepository articleRepostitory)
    {
      _repository = articleRepostitory;
    }
    [HttpGet]
    public async Task<IActionResult> Get()
    {
      var articles = await _repository.Get();
      return Ok(articles);
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
        var articles = await _repository.GetFiltered(tags);
        return Ok(articles);
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
      }catch (ItemNotFoundException)
      {
        return NotFound();
      }
      return Ok();
    }
    [HttpPost]
    public async Task<IActionResult> Insert(Article article)
    {
      article.ArticleId = 0;
      article = await _repository.Insert(article);
      return Ok(article);
    }
    [HttpPut]
    public async Task<IActionResult> Replace(Article article)
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

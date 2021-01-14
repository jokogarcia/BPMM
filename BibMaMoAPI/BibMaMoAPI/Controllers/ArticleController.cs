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
    public async Task<IActionResult> Get(string tags="")
    {
      var articles = await _repository.Get(tags);
      return Ok(articles);
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
      }catch (ItemNotFoundException)
      {
        return NotFound();
      }
      return Ok();
    }
    [HttpPost]
    public async Task<IActionResult> Insert(Article article)
    {
      article.Handle = string.Empty;
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

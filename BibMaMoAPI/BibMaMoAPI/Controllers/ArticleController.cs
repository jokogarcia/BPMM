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
    IArticleRepository _articleRepository;
    public ArticleController(IArticleRepository articleRepostitory)
    {
      _articleRepository = articleRepostitory;
    }
    [HttpGet]
    public async Task<IActionResult> GetArticles(string tags="")
    {
      var articles = await _articleRepository.GetArticles(tags);
      return Ok(articles);
    }
    [HttpGet("{handle}")]
    public async Task<IActionResult> GetSingleArticle(string handle)
    {
      try
      {
        return Ok(await _articleRepository.GetArticle(handle));
      }
      catch (ItemNotFoundException)
      {
        return NotFound();
      }
    }
    [HttpDelete("{handle}")]
    public async Task<IActionResult> DeleteArticle(string handle)
    {
      try
      {
        await _articleRepository.DeleteArticle(handle);
      }catch (ItemNotFoundException)
      {
        return NotFound();
      }
      return Ok();
    }
    [HttpPost]
    public async Task<IActionResult> InsertArticle(Article article)
    {
      article.Handle = string.Empty;
      article = await _articleRepository.AddArticle(article);
      return Ok(article);
    }
    [HttpPut]
    public async Task<IActionResult> UpdateArticle(Article article)
    {
      try
      {
        article = await _articleRepository.AddArticle(article);
        return Ok(article);
      }
      catch (ItemNotFoundException)
      {
        return NotFound();
      }
      
    }

  }
}

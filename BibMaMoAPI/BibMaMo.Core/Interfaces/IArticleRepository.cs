using BibMaMo.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BibMaMo.Core.Interfaces
{
  public interface IArticleRepository
  {
    Task<IEnumerable<Article>> GetArticles(string tags="");
    Task<Article> GetArticle(string handle);
    Task DeleteArticle(string handle);
    Task<Article> AddArticle(Article article);
  }
}

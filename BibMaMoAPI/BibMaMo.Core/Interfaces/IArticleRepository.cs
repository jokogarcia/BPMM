using BibMaMo.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BibMaMo.Core.Interfaces
{
  public interface IArticleRepository
  {
    Task<IEnumerable<Article>> Get(string tags="");
    Task<Article> GetSingle(string handle);
    Task Remove(string handle);
    Task<Article> Insert(Article article);
    Task Replace(Article article);
  }
}

using BibMaMo.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BibMaMo.Core.Interfaces
{
  public interface IArticleRepository
  {
    Task<IEnumerable<Article>> Get();
    Task<IEnumerable<Article>> GetFiltered(string tags);
    Task<Article> GetSingle(int id);
    Task Remove(int id);
    Task<Article> Insert(Article article);
    Task Replace(Article article);
  }
}

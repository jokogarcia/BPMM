using BibMaMo.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BibMaMo.Core.Interfaces
{
  public interface IBookRepository
  {
    Task<IEnumerable<Book>> Get();
    Task<IEnumerable<Book>> GetFiltered(string tags);
    Task<Book> GetSingle(string handle);
    Task Remove(string handle);
    Task<Book> Insert(Book article);
    Task Replace(Book article);
  }
}

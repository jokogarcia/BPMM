using BibMaMo.Core.DTOs;
using BibMaMo.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BibMaMo.Core.Interfaces
{
  public interface IBookRepository
  {
    Task<IEnumerable<Book>> Get();
    Task<IEnumerable<Book>> GetFilteredWithTags(string tags);
    Task<Book> GetSingle(int id);
    Task Remove(int id);
    Task<Book> Insert(Book article);
    Task Replace(Book article);
    Task<FilteredBooksResult> GetFiltered(string author, string title, string categories, int pagesize, int pagenumber);
    Task<IEnumerable<string>> GetCategories();
  }
}

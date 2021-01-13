using BibMaMo.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BibMaMo.Core.Interfaces
{
  public interface IBookRepository
  {
    Task<IEnumerable<Book>> GetBooks();
    Task<IEnumerable<Book>> GetBooksByTags(string tags);
    Task<Book> GetBook(string handle);
    Task DeleteBook(string handle);
    Task<Book> AddBook(Book Book);
  }
}

using BibMaMo.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BibMaMo.Core.Interfaces
{
  public interface IUserRepository
  {
    Task<IEnumerable<User>> Get();
    Task<User> GetSingle(string handle);
    Task Remove(string handle);
    Task<User> Insert(User article);
    Task Replace(User article);
  }
}

using BibMaMo.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BibMaMo.Core.Interfaces
{
  public interface IUserRepository
  {
    Task<IEnumerable<User>> GetUsers();
    Task<User> GetUser(string handle);
    Task DeleteUser(string handle);
    Task<User> AddUser(User User);
  }
}

using BibMaMo.Core.Entities;
using BibMaMo.Core.Exceptions;
using BibMaMo.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibMaMo.Infrastructure.Repositories
{

  public class UserMockRepository : IUserRepository
  {
    List<User> MockRepo;
    public UserMockRepository()
    {
      MockRepo = new List<User>();
      for (int x = 0; x < 10; x++)
      {
        MockRepo.Add(new User
        {
          Handle = x.ToString(),
          Email = $"user{x}@fakemail.com",
          FirstName = $"User{x}First",
          LastName = $"User{x}Last]",
          MemberId = x < 5 ? string.Empty : x.ToString()
        }); ;
      }



    }
    public Task<User> Insert(User user)
    {
      user.Handle = Guid.NewGuid().ToString();
      MockRepo.Add(user);

      return Task.FromResult(user);
    }

    public Task Remove(string handle)
    {
      var user = GetItemOrThrow(handle);
      MockRepo.Remove(user);
      return Task.CompletedTask;
    }

    public Task<User> GetSingle(string handle)
    {
      return Task.FromResult(GetItemOrThrow(handle));
    }

    public Task<IEnumerable<User>> Get(string tags = "")
    {
      IEnumerable<User> users;
      users = (IEnumerable<User>)MockRepo; //Tags is ignored for users
      return Task.FromResult(users);

    }
    private User GetItemOrThrow(string handle)
    {
      var user = MockRepo.Find(x => x.Handle.Equals(handle));
      if (user == null)
      {
        throw new ItemNotFoundException();
      }
      return user;
    }
    public Task Replace(User user)
    {
      var oldUser = GetItemOrThrow(user.Handle);
      var oldUserIndex = MockRepo.IndexOf(oldUser);
      MockRepo[oldUserIndex] = user;
      return Task.CompletedTask;
    }
  }
}

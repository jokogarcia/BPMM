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
    static List<User> _mockRepo;
    List<User> MockRepo { get => _mockRepo ?? (_mockRepo = populateMockRepo()); }

    private List<User> populateMockRepo()
    {
      var MockRepo = new List<User>();
      for (int x = 0; x < 10; x++)
      {
        MockRepo.Add(new User
        {
          Handle = x.ToString(),
          Email = $"item{x}@fakemail.com",
          FirstName = $"User{x}First",
          LastName = $"User{x}Last]",
          MemberId = x < 5 ? string.Empty : x.ToString()
        });
      }
      return MockRepo;
    }

   
    public Task<User> Insert(User item)
    {
      item.Handle = Guid.NewGuid().ToString();
      MockRepo.Add(item);

      return Task.FromResult(item);
    }

    public Task Remove(string handle)
    {
      var item = GetItemOrThrow(handle);
      MockRepo.Remove(item);
      return Task.CompletedTask;
    }

    public Task<User> GetSingle(string handle)
    {
      return Task.FromResult(GetItemOrThrow(handle));
    }

    public Task<IEnumerable<User>> Get()
    {
      IEnumerable<User> items;
      items = (IEnumerable<User>)MockRepo; 
      return Task.FromResult(items);

    }
    private User GetItemOrThrow(string handle)
    {
      var item = MockRepo.Find(x => x.Handle.Equals(handle));
      if (item == null)
      {
        throw new ItemNotFoundException();
      }
      return item;
    }
    public Task Replace(User item)
    {
      var oldUser = GetItemOrThrow(item.Handle);
      var oldUserIndex = MockRepo.IndexOf(oldUser);
      MockRepo[oldUserIndex] = item;
      return Task.CompletedTask;
    }
  }
}

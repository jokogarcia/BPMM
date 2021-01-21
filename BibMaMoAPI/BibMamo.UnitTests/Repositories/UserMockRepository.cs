using BibMaMo.Core.Entities;
using BibMaMo.Core.Exceptions;
using BibMaMo.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibMaMo.UnitTests.Repositories
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
          UserId = x,
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
      item.UserId = new Random().Next(1,100);
      MockRepo.Add(item);

      return Task.FromResult(item);
    }

    public Task Remove(int id)
    {
      var item = GetItemOrThrow(id);
      MockRepo.Remove(item);
      return Task.CompletedTask;
    }

    public Task<User> GetSingle(int id)
    {
      return Task.FromResult(GetItemOrThrow(id));
    }

    public Task<IEnumerable<User>> Get()
    {
      IEnumerable<User> items;
      items = (IEnumerable<User>)MockRepo; 
      return Task.FromResult(items);

    }
    private User GetItemOrThrow(int id)
    {
      var item = MockRepo.Find(x => x.UserId.Equals(id));
      if (item == null)
      {
        throw new ItemNotFoundException();
      }
      return item;
    }
    public Task Replace(User item)
    {
      var oldUser = GetItemOrThrow(item.UserId);
      var oldUserIndex = MockRepo.IndexOf(oldUser);
      MockRepo[oldUserIndex] = item;
      return Task.CompletedTask;
    }
  }
}

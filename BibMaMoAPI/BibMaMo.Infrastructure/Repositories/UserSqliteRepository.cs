using BibMaMo.Core.Entities;
using BibMaMo.Core.Exceptions;
using BibMaMo.Core.Interfaces;
using BibMaMo.Infrastructure.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibMaMo.Infrastructure.Repositories
{
  public class UserSqliteRepository : IUserRepository
  {



    public async Task<IEnumerable<User>> Get()
    {
      using (var context = new BPMMContext())
      {
        var allItems = context.Users.AsAsyncEnumerable();
        return await allItems.ToListAsync();
      }
    }




    public Task<User> GetSingle(int id)
    {
      return GetItemOrThrow(id);
    }

    public Task<User> Insert(User user)
    {
      using (var context = new BPMMContext())
      {
        var entity = context.Users.Add(user);
        entity.State = Microsoft.EntityFrameworkCore.EntityState.Added;
        context.SaveChanges();
        return Task.FromResult(user);
      }
    }

    public async Task Remove(int id)
    {
      using (var context = new BPMMContext())
      {
        var entity = context.Users.Remove(await this.GetItemOrThrow(id));
        entity.State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
        await context.SaveChangesAsync();
      }
    }

    public async Task Replace(User user)
    {
      using (var context = new BPMMContext())
      {
        var entity = context.Users.Update(user);
        entity.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        try
        {
          await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
          throw new ItemNotFoundException();
        }
      }
    }
    
    private async Task<User> GetItemOrThrow(int id)
    {
      using (var context = new BPMMContext())
      {
        var item = await context.Users.FindAsync(id);
        if (item == null)
        {
          throw new ItemNotFoundException();
        }
        return item;
      }

    }
  }
}

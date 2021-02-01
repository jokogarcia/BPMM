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
  public class LoanSqliteRepository : ILoanRepository
  {



    public async Task<IEnumerable<Loan>> Get()
    {
      using (var context = new BPMMContext())
      {
        var allItems = context.Loans.AsAsyncEnumerable();
        return await allItems.ToListAsync();
      }
    }
    public Task<Loan> GetSingle(int id)
    {
      return GetItemOrThrow(id);
    }

    public Task<Loan> Insert(Loan Loan)
    {
      using (var context = new BPMMContext())
      {
        var entity = context.Loans.Add(Loan);
        entity.State = Microsoft.EntityFrameworkCore.EntityState.Added;
        context.SaveChanges();
        return Task.FromResult(Loan);
      }
    }

    public async Task Remove(int id)
    {
      using (var context = new BPMMContext())
      {
        var entity = context.Loans.Remove(await this.GetItemOrThrow(id));
        entity.State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
        await context.SaveChangesAsync();
      }
    }

    public async Task Replace(Loan loan)
    {
      using (var context = new BPMMContext())
      {
        var entity = context.Loans.Update(loan);
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
   
    private async Task<Loan> GetItemOrThrow(int id)
    {
      using (var context = new BPMMContext())
      {
        var item = await context.Loans.FindAsync(id);
        if (item == null)
        {
          throw new ItemNotFoundException();
        }
        return item;
      }

    }

    public async Task<IEnumerable<Loan>> GetByBook(int bookId)
    {
      using (var context = new BPMMContext())
      {
        var items = context.Loans.AsQueryable().Where(x => x.BookId == bookId).ToList();
        return items;
      }
    }

    public async Task<IEnumerable<Loan>> GetByUser(int userId)
    {
      using (var context = new BPMMContext())
      {
        var items = context.Loans.AsQueryable().Where(x => x.UserId == userId).ToList();
        return items;
      }
    }
  }
}

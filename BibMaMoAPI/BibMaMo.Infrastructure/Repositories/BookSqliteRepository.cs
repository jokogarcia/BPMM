using BibMaMo.Core.DTOs;
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
using BibMaMo.Core.Extensions;
using System.Globalization;

namespace BibMaMo.Infrastructure.Repositories
{
  public class BookSqliteRepository : IBookRepository
  {



    public async Task<IEnumerable<Book>> Get()
    {
      using (var context = new BPMMContext())
      {
        var allItems = context.Books.AsAsyncEnumerable();
        return await allItems.ToListAsync();
      }
    }

    public async Task<IEnumerable<Book>> GetFilteredWithTags(string tags)
    {
      using (var context = new BPMMContext())
      {
        var allItems = context.Books.AsAsyncEnumerable();

        var result = await allItems.Where(x => ItemContainsTags(x, tags))
          .ToListAsync();
        if (!result.Any())
        {
          throw new ItemNotFoundException();
        }
        return result;

      }
    }



    public Task<Book> GetSingle(int id)
    {
      return GetItemOrThrow(id);
    }

    public Task<Book> Insert(Book Book)
    {
      using (var context = new BPMMContext())
      {
        var entity = context.Books.Add(Book);
        entity.State = Microsoft.EntityFrameworkCore.EntityState.Added;
        context.SaveChanges();
        return Task.FromResult(Book);
      }
    }

    public async Task Remove(int id)
    {
      using (var context = new BPMMContext())
      {
        var entity = context.Books.Remove(await this.GetItemOrThrow(id));
        entity.State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
        await context.SaveChangesAsync();
      }
    }

    public async Task Replace(Book book)
    {
      using (var context = new BPMMContext())
      {
        var entity = context.Books.Update(book);
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
    private bool ItemContainsTags(Book item, string tags)
    {
      if (string.IsNullOrEmpty(tags) || item == null || string.IsNullOrEmpty(item.Tags))
      {
        return false;
      }
      var tagsArr = tags.ToLower().Split(',');
      var itemTagsArr = item.Tags.ToLower().Split(',');
      foreach (var tag in tagsArr)
      {
        if (itemTagsArr.Contains(tag))
          return true;
      }
      return false;
    }
    private async Task<Book> GetItemOrThrow(int id)
    {
      using (var context = new BPMMContext())
      {
        var item = await context.Books.FindAsync(id);
        if (item == null)
        {
          throw new ItemNotFoundException();
        }
        return item;
      }

    }

    public async Task<FilteredBooksResult> GetFiltered(string author, string title, int pagesize, int pagenumber)
    {
      var filtered = (await Get())
        .Where(x => StringContains(x.Author, author))
        .Where(x => StringContains(x.Title, title));

      return new FilteredBooksResult
      {
        Results = filtered.Skip(pagenumber * pagesize).Take(pagesize),
        TotalCount = filtered.Count()
      };
    }
    private bool StringContains(string bigString, string littleString)
    {
      if (string.IsNullOrEmpty(littleString))
      {
        return true;
      }
      if (string.IsNullOrEmpty(bigString))
      {
        return false;
      }
      bigString = RemoveDiacritics(bigString.ToLower());
      littleString = RemoveDiacritics(littleString.ToLower());
      return bigString.Contains(littleString);
    }
    private string RemoveDiacritics(string s)
    {
      String normalizedString = s.Normalize(NormalizationForm.FormD);
      StringBuilder stringBuilder = new StringBuilder();

      for (int i = 0; i < normalizedString.Length; i++)
      {
        Char c = normalizedString[i];
        if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
          stringBuilder.Append(c);
      }

      return stringBuilder.ToString();
    }
    public async Task<IEnumerable<string>> GetCategories()
    {
      using (var context = new BPMMContext())
      {
        var result = context.Books.AsQueryable().Select(book => book.Section).Distinct();
        return result.ToList();
      }
    }

    
  }
}

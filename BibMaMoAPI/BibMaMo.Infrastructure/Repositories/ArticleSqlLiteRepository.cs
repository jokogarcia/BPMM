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
  public class ArticleSqliteRepository : IArticleRepository
  {



    public async Task<IEnumerable<Article>> Get()
    {
      using (var context = new BPMMContext())
      {
        var allItems = context.Articles.AsAsyncEnumerable();
        return await allItems.ToListAsync();
       }
      }

    public async Task<IEnumerable<Article>> GetFiltered(string tags)
    {
      using (var context = new BPMMContext())
      {
        try
        {
          var allItems = context.Articles.AsAsyncEnumerable();
          var result = await allItems.Where(x => ItemContainsTags(x, tags))
            .ToListAsync();
          if (!result.Any())
          {
            throw new ItemNotFoundException();
          }
          return result;
        }
        catch (NullReferenceException)
        {
          throw new ItemNotFoundException();
        }

      }
    }

   

    public  Task<Article> GetSingle(int id)
    {
      return GetItemOrThrow(id);
    }

    public Task<Article> Insert(Article Article)
    {
      using (var context = new BPMMContext())
      {
        var entity = context.Articles.Add(Article);
        entity.State = Microsoft.EntityFrameworkCore.EntityState.Added;
        context.SaveChanges();
        return Task.FromResult(Article);
      }
    }

    public async Task Remove(int id)
    {
      using (var context = new BPMMContext())
      {
        var entity = context.Articles.Remove(await this.GetItemOrThrow(id));
        entity.State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
        await context.SaveChangesAsync();
      }
    }

    public async Task Replace(Article article)
    {
      using (var context = new BPMMContext())
      {
        var entity = context.Articles.Update(article);
        entity.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        try {
          await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
          throw new ItemNotFoundException();
        }
      }
    }
    private bool ItemContainsTags(Article item, string tags)
    {
      if (string.IsNullOrEmpty(tags) || item==null || string.IsNullOrEmpty(item.Tags))
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
    private async Task<Article> GetItemOrThrow(int id)
    {
      using (var context = new BPMMContext())
      {
        var item = await context.Articles.FindAsync(id);
        if (item == null)
        {
          throw new ItemNotFoundException();
        }
        return item;
      }
        
    }
  }
}

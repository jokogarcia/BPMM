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

  public class ArticleMockRepository:IArticleRepository
  {
    static List<Article> _mockRepo;
    List<Article> MockRepo { get => _mockRepo ?? (_mockRepo = populateMockRepo()); }

    private List<Article> populateMockRepo()
    {
      var repo = new List<Article>();
      for (int x = 0; x < 10; x++)
      {
        repo.Add(new Article
        {
          Handle = x.ToString(),
          HtmlContent = $"Contenido del articulo {x}",
          MainImageUrl = $"tapa{x}.jpg",
          Title = $"Libro {x}",
          Tags = "coleccion"
        });
      }
      for (int x = 0; x < 10; x++)
      {
        repo.Add(new Article
        {
          Handle = (x + 10).ToString(),
          HtmlContent = $"Contenido del articulo {x + 10}",
          MainImageUrl = $"nuestros{x}.jpg",
          Title = $"Escritor {x}",
          Tags = "nuestros"
        });
      }
      return repo;
    }


    public Task<Article> Insert(Article item)
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

    public Task<Article> GetSingle(string handle)
    {
      return Task.FromResult(GetItemOrThrow(handle));
    }

    public Task<IEnumerable<Article>> Get()
    {
      IEnumerable<Article> items;
      items = (IEnumerable<Article>)MockRepo;
      return Task.FromResult(items);

    }
    public Task<IEnumerable<Article>> GetFiltered(string tags)
    {

      IEnumerable<Article> items;
      items = (IEnumerable<Article>)MockRepo.FindAll(x => ItemContainsTags(tags, x));
      if (items.Count() == 0)
      {
        throw new ItemNotFoundException();
      }
      return Task.FromResult(items);

    }
    private Article GetItemOrThrow(string handle)
    {
      var item = MockRepo.Find(x => x.Handle.Equals(handle));
      if(item == null)
      {
        throw new ItemNotFoundException();
      }
      return item;
    }

   
    private bool ItemContainsTags(string tags, Article item)
    {
      var tagsArr = tags.ToLower().Split(',');
      var itemTagsArr=item.Tags.ToLower().Split(',');
      foreach(var tag in tagsArr)
      {
        if (itemTagsArr.Contains(tag))
          return true;
      }
      return false;
    }

    public Task Replace(Article item)
    {
      var oldArticle = GetItemOrThrow(item.Handle);
      var oldArticleIndex = MockRepo.IndexOf(oldArticle);
      MockRepo[oldArticleIndex] = item;
      return Task.CompletedTask;
    }
  }
}

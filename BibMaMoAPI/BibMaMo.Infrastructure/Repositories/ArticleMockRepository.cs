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
    List<Article> MockRepo;
    public ArticleMockRepository()
    {
      MockRepo = new List<Article>();
      for(int x = 0; x < 10; x++)
      {
        MockRepo.Add(new Article {
          Handle = x.ToString(),
          HtmlContent = $"Contenido del articulo {x}",
          MainImageUrl = $"tapa{x}.jpg",
          Title=$"Libro {x}",
          Tags = "coleccion"
        });
      }
      for (int x = 0; x < 10; x++)
      {
        MockRepo.Add(new Article
        {
          Handle =(x+10).ToString(),
          HtmlContent = $"Contenido del articulo {x+10}",
          MainImageUrl = $"nuestros{x}.jpg",
          Title =$"Escritor {x}",
          Tags = "nuestros"
        });
      }

     
    }
    public Task<Article> AddArticle(Article article)
    {
      if (string.IsNullOrEmpty(article.Handle))
      {
        article.Handle = Guid.NewGuid().ToString();
      }
      var i = MockRepo.FindIndex(x => x.Handle == article.Handle);
      if (i >= 0)
      {
        MockRepo[i] = article;
      }
      else
      {
        MockRepo.Add(article);
      }
      return Task.FromResult(article);
    }

    public Task DeleteArticle(string handle)
    {
      var article = GetItemOrThrow(handle);
      MockRepo.Remove(article);
      return Task.CompletedTask;
    }

    public Task<Article> GetArticle(string handle)
    {
      return Task.FromResult(GetItemOrThrow(handle));
    }

    public Task<IEnumerable<Article>> GetArticles(string tags ="")
    {
      IEnumerable<Article> articles;
      if (string.IsNullOrEmpty(tags))
      {
        articles = (IEnumerable<Article>)MockRepo;
      }
      else
      {
        articles = (IEnumerable<Article>)MockRepo.FindAll(x => ArticleContainsTags(tags, x));
      }
      return Task.FromResult(articles);

    }
    private Article GetItemOrThrow(string handle)
    {
      var article = MockRepo.Find(x => x.Handle.Equals(handle));
      if(article == null)
      {
        throw new ItemNotFoundException();
      }
      return article;
    }

   
    private bool ArticleContainsTags(string tags, Article article)
    {
      var tagsArr = tags.ToLower().Split(',');
      var articleTagsArr=article.Tags.ToLower().Split(',');
      foreach(var tag in tagsArr)
      {
        if (articleTagsArr.Contains(tag))
          return true;
      }
      return false;
    }
  }
}
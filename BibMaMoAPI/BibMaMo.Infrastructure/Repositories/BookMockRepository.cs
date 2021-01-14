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

  public class BookMockRepository : IBookRepository
  {
    List<Book> MockRepo;
    public BookMockRepository()
    {
      MockRepo = new List<Book>();
      for (int x = 0; x < 10; x++)
      {
        MockRepo.Add(new Book
        {
          Handle = x.ToString(),
          Author=$"Autor {x}",
          Title = $"Libro {x}",
          Tags = "",
          Descriptor="novelas",
          Edition=2000+x,
          ISBN=Guid.NewGuid().ToString(),
          Pages=700+x*17,
          InventoryId=$"INV{(300+x)}",
          Publisher= x%2==0 ? "Alfaguara":"Kapeluz",
          Section = x % 2 == 0 ? "Seccion A" : "Seccion B",
          Summary="Lorem ipsum doloor."
        });
      }
      


    }
    public Task<Book> Insert(Book book)
    {
      book.Handle = Guid.NewGuid().ToString();
      MockRepo.Add(book);

      return Task.FromResult(book);
    }

    public Task Remove(string handle)
    {
      var book = GetItemOrThrow(handle);
      MockRepo.Remove(book);
      return Task.CompletedTask;
    }

    public Task<Book> GetSingle(string handle)
    {
      return Task.FromResult(GetItemOrThrow(handle));
    }

    public Task<IEnumerable<Book>> Get(string tags = "")
    {
      IEnumerable<Book> books;
      if (string.IsNullOrEmpty(tags))
      {
        books = (IEnumerable<Book>)MockRepo;
      }
      else
      {
        books = (IEnumerable<Book>)MockRepo.FindAll(x => ItemContainsTags(tags, x));
      }
      return Task.FromResult(books);

    }
    private Book GetItemOrThrow(string handle)
    {
      var book = MockRepo.Find(x => x.Handle.Equals(handle));
      if (book == null)
      {
        throw new ItemNotFoundException();
      }
      return book;
    }


    private bool ItemContainsTags(string tags, Book book)
    {
      var tagsArr = tags.ToLower().Split(',');
      var bookTagsArr = book.Tags.ToLower().Split(',');
      foreach (var tag in tagsArr)
      {
        if (bookTagsArr.Contains(tag))
          return true;
      }
      return false;
    }

    public Task Replace(Book book)
    {
      var oldBook = GetItemOrThrow(book.Handle);
      var oldBookIndex = MockRepo.IndexOf(oldBook);
      MockRepo[oldBookIndex] = book;
      return Task.CompletedTask;
    }
  }
}

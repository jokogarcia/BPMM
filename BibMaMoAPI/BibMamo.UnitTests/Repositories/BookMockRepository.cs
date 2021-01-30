using BibMaMo.Core.DTOs;
using BibMaMo.Core.Entities;
using BibMaMo.Core.Exceptions;
using BibMaMo.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibMaMo.UnitTests.Repositories
{

  public class BookMockRepository : IBookRepository
  {
    static List<Book> _mockRepo;
    List<Book> MockRepo { get => _mockRepo ?? (_mockRepo = populateMockRepo()); }

    private List<Book> populateMockRepo()
    {
      var repo = new List<Book>();
      for (int x = 0; x < 10; x++)
      {
        repo.Add(new Book
        {
          BookId = x,
          Author = $"Autor {x}",
          Title = $"Libro {x}",
          Tags = "",
          Descriptor = "novelas",
          Edition = 2000 + x,
          ISBN = Guid.NewGuid().ToString(),
          Pages = 700 + x * 17,
          InventoryId = $"INV{(300 + x)}",
          Publisher = x % 2 == 0 ? "Alfaguara" : "Kapeluz",
          Section = x % 2 == 0 ? "Seccion A" : "Seccion B",
          Summary = "Lorem ipsum doloor."
        });
      }
      for (int x = 10; x < 15; x++)
      {
        repo.Add(new Book
        {
          BookId = x,
          Author = $"Autor {x}",
          Title = $"Libro {x}",
          Tags = "some",
          Descriptor = "cuentos",
          Edition = 2000 + x,
          ISBN = Guid.NewGuid().ToString(),
          Pages = 700 + x * 17,
          InventoryId = $"INV{(300 + x)}",
          Publisher = x % 2 == 0 ? "Alfaguara" : "Kapeluz",
          Section = x % 2 == 0 ? "Seccion A" : "Seccion B",
          Summary = "Lorem ipsum doloor."
        });
      }
      for (int x = 15; x < 20; x++)
      {
        repo.Add(new Book
        {
          BookId = x,
          Author = $"Autor {x}",
          Title = $"Libro {x}",
          Tags = "tags",
          Descriptor = "poesia",
          Edition = 2000 + x,
          ISBN = Guid.NewGuid().ToString(),
          Pages = 700 + x * 17,
          InventoryId = $"INV{(300 + x)}",
          Publisher = x % 2 == 0 ? "Alfaguara" : "Kapeluz",
          Section = x % 2 == 0 ? "Seccion A" : "Seccion B",
          Summary = "Lorem ipsum doloor."
        });
      }
      return repo;
    }


    public Task<Book> Insert(Book book)
    {
      book.BookId = new Random().Next(1, 100);
      MockRepo.Add(book);

      return Task.FromResult(book);
    }

    public Task Remove(int id)
    {
      var book = GetItemOrThrow(id);
      MockRepo.Remove(book);
      return Task.CompletedTask;
    }

    public Task<Book> GetSingle(int id)
    {
      return Task.FromResult(GetItemOrThrow(id));
    }

    public Task<IEnumerable<Book>> Get()
    {
      IEnumerable<Book> items;
      items = (IEnumerable<Book>)MockRepo;
      return Task.FromResult(items);

    }
    public Task<IEnumerable<Book>> GetFilteredWithTags(string tags)
    {

      IEnumerable<Book> items;
      items = (IEnumerable<Book>)MockRepo.FindAll(x => ItemContainsTags(tags, x));
      if (items.Count() == 0)
      {
        throw new ItemNotFoundException();
      }
      return Task.FromResult(items);

    }
    private Book GetItemOrThrow(int id)
    {
      var book = MockRepo.Find(x => x.BookId.Equals(id));
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
      var oldBook = GetItemOrThrow(book.BookId);
      var oldBookIndex = MockRepo.IndexOf(oldBook);
      MockRepo[oldBookIndex] = book;
      return Task.CompletedTask;
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

    public IEnumerable<string> GetCategories()
    {
      throw new NotImplementedException();
    }

    async Task<IEnumerable<string>> IBookRepository.GetCategories()
    {
      var result = (await this.Get()).Select(book => book.Section).Distinct();
      return result.ToList();
    }
  }
}

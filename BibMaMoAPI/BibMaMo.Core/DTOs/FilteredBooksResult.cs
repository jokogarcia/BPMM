using BibMaMo.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BibMaMo.Core.DTOs
{
  public class FilteredBooksResult
  {
    public int TotalCount { get; set; }
    public IEnumerable<Book> Results { get; set; }
  }
}

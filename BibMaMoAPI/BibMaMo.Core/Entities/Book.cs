using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BibMaMo.Core.Entities
{
  public class Book
  {
    [Key]
    public int BookId { get; set; }
    public string Author { get; set; }
    public string Title { get; set; }
    public string ISBN { get; set; }
    public string InventoryId { get; set; }
    public string Summary { get; set; }
    public string Publisher { get; set; }
    public int Edition { get; set; }
    public int Pages { get; set; }
    public string Section { get; set; }
    public string Collection { get; set; }
    public string Descriptor { get; set; }
    public string Tags { get; set; }



  }
}

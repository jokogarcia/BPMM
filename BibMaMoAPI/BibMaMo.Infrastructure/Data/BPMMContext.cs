using BibMaMo.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BibMaMo.Infrastructure.Data
{
  public class BPMMContext:DbContext
  {
    public BPMMContext()
    {
      this.Database.EnsureCreated();
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseSqlite("Filename=C:\\Repositories\\BPMM\\BibMaMoAPI\\BibMaMoAPI\\bpmmm.sqlite");

    }
   
    public DbSet<Book> Books { get; set; }
    public DbSet<Article> Articles { get; set; }
    public DbSet<User> Users { get; set; }
  }
}

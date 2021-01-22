using BibMaMo.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
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
      var filename = "bpmm.sqlite";
      var homefolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
      var fullpath = Path.Combine(homefolder, filename);
#if DEBUG
      optionsBuilder.UseSqlite($"Filename={fullpath}");
#else
      optionsBuilder.UseSqlite($"Filename=./{filename}");
#endif
    }

    public DbSet<Book> Books { get; set; }
    public DbSet<Article> Articles { get; set; }
    public DbSet<User> Users { get; set; }
  }
}

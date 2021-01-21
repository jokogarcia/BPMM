using System.ComponentModel.DataAnnotations;

namespace BibMaMo.Core.Entities
{
  public class Article
  {
    [Key]
    public int ArticleId { get; set; }
    public string Tags { get; set; }
    public string HtmlContent { get; set; }
    public string Title { get; set; }
    public string MainImageUrl { get; set; }

  }
}

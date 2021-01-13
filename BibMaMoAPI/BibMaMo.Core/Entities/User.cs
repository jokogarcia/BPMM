using System;
using System.Collections.Generic;
using System.Text;

namespace BibMaMo.Core.Entities
{
  public class User
  {
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string MemberId { get; set; }
    public string Handle { get; set; }
  }
}

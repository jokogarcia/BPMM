using System;
using System.Collections.Generic;
using System.Text;

namespace BibMaMo.Core.Entities
{
  public class Loan
  {
    public int LoanId { get; set; }
    public int BookId { get; set; }
    public int UserId { get; set; }
    public DateTime BorrowDate { get; set; }
    public DateTime ReturnDueDate { get; set; }
    public DateTime? ReturnedDate { get; set; }
  }
}

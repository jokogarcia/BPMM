using BibMaMo.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BibMaMo.Core.Interfaces
{
  public interface ILoanRepository
  {
    Task<IEnumerable<Loan>> Get();
    Task<IEnumerable<Loan>> GetByBook(int bookId);
    Task<IEnumerable<Loan>> GetByUser(int userId);
    Task<Loan> GetSingle(int id);
    Task Remove(int id);
    Task<Loan> Insert(Loan loan);
    Task Replace(Loan loan);
  }
}

using BibMaMo.Core.Entities;
using BibMaMo.Core.Exceptions;
using BibMaMo.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibMaMo.UnitTests.Repositories
{

  public class LoanMockRepository : ILoanRepository
  {
    static List<Loan> _mockRepo;
    List<Loan> MockRepo { get => _mockRepo ?? (_mockRepo = populateMockRepo()); }
    private Random gen = new Random();
    DateTime? RandomDay(DateTime? minDate, int maxOffset=1000)
    {
      return ((DateTime)minDate).AddDays(gen.Next(30,maxOffset));
    }

    private List<Loan> populateMockRepo()
    {
      var MockRepo = new List<Loan>();
      for (int x = 0; x < 10; x++)
      {
        var loanDate = RandomDay(DateTime.Now.AddYears(-1));
        var dueDate = RandomDay(loanDate);
        DateTime? returnDate = (gen.Next(100) >= 50 ? RandomDay(loanDate) : null);
        MockRepo.Add(new Loan
        {
          LoanId = x,
         BookId= gen.Next(0,10),
         UserId = gen.Next(0, 10),
         BorrowDate = (DateTime)loanDate,
         ReturnDueDate=(DateTime)dueDate,
         ReturnedDate=returnDate

        });
      }
      return MockRepo;
    }


    public Task<Loan> Insert(Loan item)
    {
      item.LoanId = new Random().Next(1, 100);
      MockRepo.Add(item);

      return Task.FromResult(item);
    }

    public Task Remove(int id)
    {
      var item = GetItemOrThrow(id);
      MockRepo.Remove(item);
      return Task.CompletedTask;
    }

    public Task<Loan> GetSingle(int id)
    {
      return Task.FromResult(GetItemOrThrow(id));
    }

    public Task<IEnumerable<Loan>> Get()
    {
      IEnumerable<Loan> items;
      items = (IEnumerable<Loan>)MockRepo;
      return Task.FromResult(items);

    }
    private Loan GetItemOrThrow(int id)
    {
      var item = MockRepo.Find(x => x.LoanId.Equals(id));
      if (item == null)
      {
        throw new ItemNotFoundException();
      }
      return item;
    }
    public Task Replace(Loan item)
    {
      var oldLoan = GetItemOrThrow(item.LoanId);
      var oldLoanIndex = MockRepo.IndexOf(oldLoan);
      MockRepo[oldLoanIndex] = item;
      return Task.CompletedTask;
    }

    public async Task<IEnumerable<Loan>> GetByBook(int bookId)
    {
      var allItems = await this.Get();
      return allItems.Where(x => x.BookId == bookId);
    }

    public async Task<IEnumerable<Loan>> GetByUser(int userId)
    {
      var allItems = await this.Get();
      return allItems.Where(x => x.UserId == userId);
    }
  }
}

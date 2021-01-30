using BibMaMo.Core.Entities;
using BibMaMo.Core.Exceptions;
using BibMaMo.Core.Interfaces;
using BibMaMo.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BibMaMo.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class LoanController : ControllerBase
  {
    ILoanRepository _repository;
    public LoanController(ILoanRepository loanRepostitory)
    {
      _repository = loanRepostitory;
    }
    [HttpGet]
    public async Task<IActionResult> Get()
    {
      var loans = await _repository.Get();
      return Ok(loans);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSingle(int id)
    {
      try
      {
        return Ok(await _repository.GetSingle(id));
      }
      catch (ItemNotFoundException)
      {
        return NotFound();
      }
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Remove(int id)
    {
      try
      {
        await _repository.Remove(id);
      }
      catch (ItemNotFoundException)
      {
        return NotFound();
      }
      return Ok();
    }
    [HttpPost]
    public async Task<IActionResult> Insert(Loan loan)
    {
      loan.LoanId = 0;
      loan = await _repository.Insert(loan);
      return Ok(loan);
    }
    [HttpPut]
    public async Task<IActionResult> Replace(Loan loan)
    {
      try
      {
        await _repository.Replace(loan);
        return Ok();
      }
      catch (ItemNotFoundException)
      {
        return NotFound();
      }

    }
    [HttpGet("byBookId/{bookId}")]
    public async Task<IActionResult> GetByBookId(int bookId)
    {
      return Ok(await _repository.GetByBook(bookId));
    }
    [HttpGet("byUserId/{userId}")]
    public async Task<IActionResult> GetByUserId(int userId)
    {
      return Ok(await _repository.GetByUser(userId));
    }

  }
}

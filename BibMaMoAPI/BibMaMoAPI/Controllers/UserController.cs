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
  public class UserController : ControllerBase
  {
    IUserRepository _repository;
    public UserController(IUserRepository repository)
    {
      _repository = repository;
    }
    [HttpGet]
    public async Task<IActionResult> Get(string tags = "")
    {
      var Users = await _repository.Get(tags);
      return Ok(Users);
    }
    [HttpGet("{handle}")]
    public async Task<IActionResult> GetSingle(string handle)
    {
      try
      {
        return Ok(await _repository.GetSingle(handle));
      }
      catch (ItemNotFoundException)
      {
        return NotFound();
      }
    }
    [HttpDelete("{handle}")]
    public async Task<IActionResult> Remove(string handle)
    {
      try
      {
        await _repository.Remove(handle);
      }
      catch (ItemNotFoundException)
      {
        return NotFound();
      }
      return Ok();
    }
    [HttpPost]
    public async Task<IActionResult> Insert(User User)
    {
      User.Handle = string.Empty;
      User = await _repository.Insert(User);
      return Ok(User);
    }
    [HttpPut]
    public async Task<IActionResult> Replace(User User)
    {
      try
      {
        await _repository.Replace(User);
        return Ok();
      }
      catch (ItemNotFoundException)
      {
        return NotFound();
      }

    }

  }
}

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
    public UserController(IUserRepository userRepostitory)
    {
      _repository = userRepostitory;
    }
    [HttpGet]
    public async Task<IActionResult> Get()
    {
      var users = await _repository.Get();
      return Ok(users);
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
    public async Task<IActionResult> Insert(User user)
    {
      user.UserId = 0;
      user = await _repository.Insert(user);
      return Ok(user);
    }
    [HttpPut]
    public async Task<IActionResult> Replace(User user)
    {
      try
      {
        await _repository.Replace(user);
        return Ok();
      }
      catch (ItemNotFoundException)
      {
        return NotFound();
      }

    }

  }
}

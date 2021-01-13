using BibMaMo.Core.Entities;
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
    IUserRepository _UserRepository;
    public UserController(IUserRepository UserRepostitory)
    {
      _UserRepository = UserRepostitory;
    }
    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
      var Users = await _UserRepository.GetUsers();
      return Ok(Users);
    }
    [HttpGet("/{handle}")]
    public async Task<IActionResult> GetSingleUser(string handle)
    {
      return Ok(await _UserRepository.GetUser(handle));
    }
    [HttpDelete("/{handle}")]
    public async Task<IActionResult> DeleteUser(string handle)
    {
      await _UserRepository.DeleteUser(handle);
      return Ok(null);
    }
    [HttpPost]
    public async Task<IActionResult> InsertUser(User User)
    {
      User.Handle = string.Empty;
      User = await _UserRepository.AddUser(User);
      return Ok(User);
    }
    [HttpPut]
    public async Task<IActionResult> UpdateUser(User User)
    {
      User = await _UserRepository.AddUser(User);
      return Ok(User);
    }

  }
}

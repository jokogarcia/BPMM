using BibMaMo.Core.Entities;
using BibMaMo.Core.Exceptions;
using BibMaMo.Core.Interfaces;
using BibMaMo.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BibMaMo.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class SolicitudInscripcionSocioController : ControllerBase
  {
    ISolicitudInscripcionSocioRepository _repository;
    public SolicitudInscripcionSocioController(ISolicitudInscripcionSocioRepository SolicitudInscripcionSocioRepostitory)
    {
      _repository = SolicitudInscripcionSocioRepostitory;
    }
    [HttpGet]
    public async Task<IActionResult> Get()
    {
      var SolicitudInscripcionSocios = await _repository.Get();
      return Ok(SolicitudInscripcionSocios);
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
    public async Task<IActionResult> Insert(SolicitudInscripcionSocio SolicitudInscripcionSocio)
    {
      SolicitudInscripcionSocio.SolicitudId = 0;
      try
      {
        SolicitudInscripcionSocio = await _repository.Insert(SolicitudInscripcionSocio);
        return Ok(SolicitudInscripcionSocio);
      }
      catch (Exception ex)
      {
        if (ex.InnerException.Message == "SQLite Error 19: 'UNIQUE constraint failed: SolicitudInscripcionSocios.Email'.")
        {
          return StatusCode(403, "Duplicate Email");
        }
        throw;
      }
      
    }
    [HttpPut]
    public async Task<IActionResult> Replace(SolicitudInscripcionSocio SolicitudInscripcionSocio)
    {
      try
      {
        await _repository.Replace(SolicitudInscripcionSocio);
        return Ok();
      }
      catch (ItemNotFoundException)
      {
        return NotFound();
      }

    }

  }
}

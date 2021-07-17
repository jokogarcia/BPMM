using BibMaMo.Core.Entities;
using BibMaMo.Core.Exceptions;
using BibMaMo.Core.Interfaces;
using BibMaMo.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text;
using System.Threading.Tasks;

namespace BibMaMo.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class SolicitudInscripcionSocioController : ControllerBase
  {
    ISolicitudInscripcionSocioRepository _repository;
    static MailerService.Mailer _mailer;
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
    [HttpGet("estado/{estado}")]
    public async Task<IActionResult> GetFiltered(string estado)
    {
      if (string.IsNullOrEmpty(estado))
      {
        return BadRequest();
      }
      try
      {
        var items = await _repository.GetFiltered(estado);
        return Ok(items);
      }
      catch (ItemNotFoundException)
      {
        return NotFound();
      }
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
    [HttpGet("intervalo")]
    public async Task<IActionResult> GetInterval(DateTime from , DateTime to)
    {
      try
      {
        return new OkObjectResult( await _repository.GetInterval(from,to));
        
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
    public async Task<IActionResult> Insert(SolicitudInscripcionSocio solicitudInscripcionSocio)
    {
      solicitudInscripcionSocio.SolicitudId = 0;
      try
      {
        solicitudInscripcionSocio = await _repository.Insert(solicitudInscripcionSocio);
        await ComposeAndSendEmail(solicitudInscripcionSocio);
        return Ok(solicitudInscripcionSocio);
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

    private Task ComposeAndSendEmail(SolicitudInscripcionSocio solicitudInscripcionSocio)
    {
      if (_mailer == null)
      {
        _mailer = new MailerService.Mailer("smtp.hostinger.com", 465, "$9Lw<&%tH<b=E&FN", "sitio@bibliotecamarianomoreno.org", "sitio@bibliotecamarianomoreno.org", "Biblioteca Popular Mariano Moreno");
      }
     
      var subject = $"Solicitud de inscripción de socio de {solicitudInscripcionSocio.Nombre} {solicitudInscripcionSocio.Apellido}";
      var bodyBuilder = new StringBuilder($"<p>Solicitud enviada en {DateTime.Now}</p>");
      bodyBuilder.Append($"<p> Nota: </p><div class='nota'>");
      bodyBuilder.Append(solicitudInscripcionSocio.Nota);
      bodyBuilder.Append("</div>");

      return _mailer.SendEmail("jokogarcia@gmail.com", subject, bodyBuilder.ToString());

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

    [HttpPut("{id}/aprobar")]
    public async Task<IActionResult> Approve(int id)
    {
      try
      {
        await _repository.Approve(id);
        return Ok();
      }
      catch (ItemNotFoundException)
      {
        return NotFound();
      }

    }

    [HttpPut("{id}/rechazar")]
    public async Task<IActionResult> Reject(int id)
    {
      try
      {
        await _repository.Reject(id);
        await _repository.Reject(id);
        return Ok();
      }
      catch (ItemNotFoundException)
      {
        return NotFound();
      }

    }

  }
}

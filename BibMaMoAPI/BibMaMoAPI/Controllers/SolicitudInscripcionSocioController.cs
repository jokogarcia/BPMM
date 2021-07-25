using BibMaMo.Core.Entities;
using BibMaMo.Core.Exceptions;
using BibMaMo.Core.Interfaces;
using BibMaMo.Infrastructure.Repositories;
using MailerService;
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
    ISolicitudInscripcionSocioRepository _solicitudRepository;
    ICodigoVerificacionRepository _codigoRepository;
    private IMailerService _mailer;
    
    public SolicitudInscripcionSocioController(ISolicitudInscripcionSocioRepository SolicitudInscripcionSocioRepostitory, ICodigoVerificacionRepository codigoVerificacionRepository, IMailerService mailerService)
    {
      _solicitudRepository = SolicitudInscripcionSocioRepostitory;
      _codigoRepository = codigoVerificacionRepository;
      _mailer = mailerService;

    }
    [HttpGet]
    public async Task<IActionResult> Get()
    {
      var SolicitudInscripcionSocios = await _solicitudRepository.Get();
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
        var items = await _solicitudRepository.GetFiltered(estado);
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
        return Ok(await _solicitudRepository.GetSingle(id));
      }
      catch (ItemNotFoundException)
      {
        return NotFound();
      }
    }
    [HttpGet("intervalo")]
    public async Task<IActionResult> GetInterval(DateTime from, DateTime to)
    {
      try
      {
        return new OkObjectResult(await _solicitudRepository.GetInterval(from, to));

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
        await _solicitudRepository.Remove(id);
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
      solicitudInscripcionSocio.FechaCreacion = DateTime.Now;
      solicitudInscripcionSocio.Estado = "N";
      var codigo = GenerateCode();
      solicitudInscripcionSocio = await _solicitudRepository.Insert(solicitudInscripcionSocio);
      await SendCodeVerificationEmail(solicitudInscripcionSocio.Email, codigo);
      await _codigoRepository.Insert(new CodigoVerificacion() { Codigo = codigo , SolicitudId=solicitudInscripcionSocio.SolicitudId});


      return Ok(solicitudInscripcionSocio);

    }

   
    [HttpGet ("resendCode/{solicitudId}")]
    public async Task<IActionResult> ResendCode(int solicitudId)
    {
      string codigo = GenerateCode();
      try
      {
        var solicitudTask = _solicitudRepository.GetSingle(solicitudId);

        await _codigoRepository.Update(solicitudId, codigo);
        var email = (await solicitudTask).Email;
        await SendCodeVerificationEmail(email, codigo);
        return Ok();
      }catch (ItemNotFoundException)
      {
        return NotFound();
      }
    }

   

    [HttpPut("{solicitudId}/verificar/{codigo}")]
    public async Task<IActionResult> VerifyCode(int solicitudId, string codigo)
    {
      try
      {
        var codigoOb = await _codigoRepository.GetSingleBySolicitudId(solicitudId);
        if (codigo == codigoOb.Codigo)
        {
          var solicitud = await _solicitudRepository.GetSingle(solicitudId);
          if (solicitud.Estado == "N")
          {
            solicitud.Estado = "V";
            await _solicitudRepository.Replace(solicitud);
            await SendSolicitudEmail(solicitud);
          }

          return Ok();

        }
        else
        {
          return BadRequest("50 Los códigos no coinciden");
        }
      }
      catch (CodeExpiredException)
      {
        return BadRequest("51 El código ha expirado");
      }
      catch (ItemNotFoundException)
      {
        return NotFound();
      }
      
    }

  

    [HttpPut]
    public async Task<IActionResult> Replace(SolicitudInscripcionSocio SolicitudInscripcionSocio)
    {
      try
      {
        await _solicitudRepository.Replace(SolicitudInscripcionSocio);
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
        await _solicitudRepository.Approve(id);
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
        await _solicitudRepository.Reject(id);
        await _solicitudRepository.Reject(id);
        return Ok();
      }
      catch (ItemNotFoundException)
      {
        return NotFound();
      }

    }
    private string GenerateCode()
    {
      return Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 8);
    }
    private Task SendCodeVerificationEmail(string email, string codigo)
    {
      string message = $"<p>Su código de verificación es {codigo}.</p><h3>¿Por qué estoy recibiendo esto?</h3>";
      message += "<p>Usted esta recibiendo este email porque ha solicitado inscribirse como socie en la Biblioteca Popular Mariano Moreno de La Rioja (https://www.bibliotecamarianomoreno.org). Si cree que se trata de un error, puede ignorar este correo sin preocupaciones.</p>";
      string asunto = "Código de verificacion de email";
      return _mailer.SendEmail(email, asunto, message);
    }
    private Task SendSolicitudEmail(SolicitudInscripcionSocio solicitudInscripcionSocio)
    {


      var subject = $"Solicitud de inscripción de socio de {solicitudInscripcionSocio.Nombre} {solicitudInscripcionSocio.Apellido}";
      var bodyBuilder = new StringBuilder($"<p>Solicitud enviada en {DateTime.Now}</p>");
      bodyBuilder.Append($"<p> Nota: </p><div class='nota'>");
      bodyBuilder.Append(solicitudInscripcionSocio.Nota);
      bodyBuilder.Append("</div>");
      //Hacer To=null envía el email a la direccion configurada en appsettings "DefaultToAddress"
      return _mailer.SendEmail(null, subject, bodyBuilder.ToString());

    }

  }
}

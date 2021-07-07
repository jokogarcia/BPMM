using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BibMaMo.Core.Entities
{
  public class SolicitudInscripcionSocio
  {
    [Key]
    public int SolicitudId { get; set; }
    public string Apellido{get;set;}
    public string Nombre { get;set;}
    public string Email { get;set;}
    public string DniTipo { get;set;}
    public string DniNum { get;set;}
    public DateTime Fnac { get; set; }
    public string CalleHogar{get;set;}
    public string CalleNumeroHogar{get;set;}
    public string BarrioHogar { get;set;}
    public string PisoHogar { get;set;}
    public string DepartamentoHogar { get;set;}
    public string TelefonoHogar { get;set;}
    public string CalleLaboral{get;set;}
    public string CalleNumeroLaboral{get;set;}
    public string BarrioLaboral { get;set;}
    public string PisoLaboral { get;set;}
    public string DepartamentoLaboral { get;set;}
    public string TelefonoLaboral { get;set;}
    public string Estado { get; set; }

  }
}

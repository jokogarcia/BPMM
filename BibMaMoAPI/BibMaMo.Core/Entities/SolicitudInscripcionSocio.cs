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
    public DatosDeContacto DatosContactoPersonal { get; set; }
    public DatosDeContacto DatosContactoLaboral { get; set; }
    public string TipoSolicitud { get; set; }
    public string Estado { get; set; }

  }
  public class DatosDeContacto
  {
    [Key]
    public int Id { get; set; }
    public string Calle { get; set; }
    public string CalleNumero { get; set; }
    public string Barrio { get; set; }
    public string Piso { get; set; }
    public string Departamento { get; set; }
    public string Telefono { get; set; }
  }
}

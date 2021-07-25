using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BibMaMo.Core.Entities
{
  
  public class CodigoVerificacion
  {
    [Key]
    public int CodigoId { get; set; }
    public int SolicitudId { get; set; }
    public DateTime ExpirationDate { get; set; }
    public string Codigo { get; set; }
  }
 
}

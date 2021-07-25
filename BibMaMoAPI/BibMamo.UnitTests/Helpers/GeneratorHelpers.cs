using BibMaMo.Core.Entities;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace BibMamo.UnitTests.Helpers
{
  internal static class GeneratorHelpers
  {
    static RandomItemPicker Estados = new RandomItemPicker(new string[] { "N", "R", "A" });
    static RandomItemPicker Tipos = new RandomItemPicker(new string[] { "CADETE", "ORDINARIO", "BENEFACTOR" });
    private static RNGCryptoServiceProvider _rng;
    private static RNGCryptoServiceProvider Rng { get => _rng ?? (_rng = new RNGCryptoServiceProvider()); }
    public static SolicitudInscripcionSocio generateRandomSolicitud(int id)
    {
      var fnac = DateTime.Now.AddDays(-1 * GetNextInt32(365 * 12, 365 * 75));
      var fcr = RandomDateBetweenDates(fnac);
      var fpr = CoinToss() ? RandomDateBetweenDates(fcr) : DateTime.MinValue;
      return new SolicitudInscripcionSocio
      {
        SolicitudId = id,
        Email = $"item{id}@fakemail.com",
        Nombre = $"SolicitudInscripcionSocio{id}First",
        Apellido = $"SolicitudInscripcionSocio{id}Last]",
        DniNum = GetNextInt32(2500000, 60000000).ToString(),
        DniTipo = "DNI",
        Estado = Estados.Get(),
        FechaCreacion = fcr,
        FechaProcesamiento = fpr,
        Fnac = fnac,
        TipoSolicitud = Tipos.Get(),
        Nota = "Lorem ipsum",
        DatosContactoPersonal = randomDatosContactos(),
        DatosContactoLaboral = CoinToss() ? randomDatosContactos() : null
      };
    }
    public static DatosDeContacto randomDatosContactos(int id=0)
    {
      var Calles = new RandomItemPicker(new string[] { "Copiapo", "alberdi", "9 de Julio", "Santa Fe", "Huniken", "Los Granados" });
      var Barrios = new RandomItemPicker(new string[] { "", "Centro", "Vargas", "Antártida 1" });
      return new DatosDeContacto
      {
        Calle = Calles.Get(),
        Barrio = Barrios.Get(),
        CalleNumero = GetNextInt32(10,2000).ToString(),
        Id = id
      };
    }
    static bool CoinToss()
    {
      return GetNextInt32(0,10) > 5;
    }
    public static int GetNextInt32(int low, int hi)
    { return (int)GetNextInt64(low, hi); }
    public static long GetNextInt64(long low, long hi)
    {
      if (low >= hi)
        throw new ArgumentException("low must be < hi");
      byte[] buf = new byte[8];
      double num;

      //Generate a random double
      Rng.GetBytes(buf);
      num = Math.Abs(BitConverter.ToDouble(buf, 0));

      //We only use the decimal portion
      num = num - Math.Truncate(num);

      //Return a number within range
      return (long)(num * ((double)hi - (double)low) + low);
    }
    static DateTime RandomDateBetweenDates(DateTime lowLimit, DateTime? highLimit = null)
    {
      DateTime HL = highLimit ?? DateTime.Now;
      var tickDif = HL.Ticks - lowLimit.Ticks;
      if (tickDif < TimeSpan.TicksPerDay) { tickDif += TimeSpan.TicksPerDay; }
      return new DateTime(lowLimit.Ticks + GetNextInt64(TimeSpan.TicksPerDay,tickDif));
    }
    private class RandomItemPicker
    {
      private readonly string[] items;
      public RandomItemPicker(string[] items)
      {
        this.items = items;
      }
      public string Get()
      {
        return items[(GetNextInt32(0,items.Length - 1))];
      }
    }
  }
}

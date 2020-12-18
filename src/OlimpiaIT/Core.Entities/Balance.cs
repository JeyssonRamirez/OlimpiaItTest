//   -----------------------------------------------------------------------
//   <copyright file=Balance.cs company="Jeysson Ramirez">
//       Copyright (c) Jeysson Ramirez Todos los derechos reservados.
//   </copyright>
//   <author>Jeysson Stevens  Ramirez </author>
//   <Date>  2020 -12-17  - 20:23</Date>
//   <Update> 2020-12-17 - 20:23</Update>
//   -----------------------------------------------------------------------

namespace Core.Entities
{
    public class Balance
    {
        public long CuentaOrigen { get; set; }
        public string TipoTransaccion { get; set; }
        public char ValorTransaccion { get; set; }
        public double SaldoCuenta { get; set; }
        public object ExtensionData { get; set; }
        public string Titular { get; set; }
    }
}
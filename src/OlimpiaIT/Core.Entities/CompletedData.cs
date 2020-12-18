//   -----------------------------------------------------------------------
//   <copyright file=CompletedData.cs company="Jeysson Ramirez">
//       Copyright (c) Jeysson Ramirez Todos los derechos reservados.
//   </copyright>
//   <author>Jeysson Stevens  Ramirez </author>
//   <Date>  2020 -12-17  - 19:31</Date>
//   <Update> 2020-12-17 - 19:39</Update>
//   -----------------------------------------------------------------------

#region

using System.Collections.Generic;


#endregion

namespace Core.Entities
{
    public class CompletedData
    {
        public List<Balance> BalanceList { get; set; }

        public List<MyAccount> ProcessedAccounts { get; set; }
        public decimal Counter { get; set; }
        public decimal Total { get; set; }
    }
}
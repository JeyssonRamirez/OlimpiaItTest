using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class CompletedDate
    {
        public List<Saldo> BalanceList { get; set; }

        public List<MyAccount> ProcessedAccounts { get; set; }
        public decimal Counter { get; set; }
        public decimal Total { get; set; }
    }
}

//   -----------------------------------------------------------------------
//   <copyright file=ReconoserRepository.cs company="Jeysson Ramirez">
//       Copyright (c) Jeysson Ramirez Todos los derechos reservados.
//   </copyright>
//   <author>Jeysson Stevens  Ramirez </author>
//   <Date>  2020 -12-17  - 20:25</Date>
//   <Update> 2020-12-17 - 20:36</Update>
//   -----------------------------------------------------------------------

#region

using System.Collections.Generic;
using System.Linq;
using System.Net;
using Core.Entities;
using Core.GlobalRepository;

#endregion

namespace DataAccess.Provider.Reconoser
{
    public class ReconoserRepository : IReconoserRepository
    {
        public readonly ServiceClient Client;

        public ReconoserRepository()
        {
            //Trust all certificates
            ServicePointManager.ServerCertificateValidationCallback =
                ((senderOne, certificate, chain, sslPolicyErrors) => true);
            // trust sender
            //System.Net.ServicePointManager.ServerCertificateValidationCallback = ((senderOne, cert, chain, errors) => cert.Subject.Contains("YourServerName"));
            Client = new ServiceClient();
        }

        public string GetPasswordEncryptedAccount(string user, string password, long account)
        {
            return Client.GetClaveCifradoCuenta(user, password, account);
        }

        public void SaveData(string usuario, string password, List<Balance> Saldos)
        {
            Client.SaveData("usuariop", "passwordp", Saldos.Select(s => new Saldo
            {
                CuentaOrigen = s.CuentaOrigen,
                SaldoCuenta = s.SaldoCuenta,
                Titular = s.Titular
            }).ToArray());
        }

        public List<Transaction> GetData(string user, string password)
        {
            var t = Client.GetData(user, password);
            return t.Select(s => new Transaction
            {
                CuentaOrigen = s.CuentaOrigen,
                Titular = s.Titular,
                TipoTransaccion = s.TipoTransaccion,
                ValorTransaccion = s.ValorTransaccion,
            }).ToList();
        }
    }
}
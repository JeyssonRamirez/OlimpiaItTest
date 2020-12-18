//   -----------------------------------------------------------------------
//   <copyright file=IReconoserRepository.cs company="Jeysson Ramirez">
//       Copyright (c) Jeysson Ramirez Todos los derechos reservados.
//   </copyright>
//   <author>Jeysson Stevens  Ramirez </author>
//   <Date>  2020 -12-17  - 20:24</Date>
//   <Update> 2020-12-17 - 20:33</Update>
//   -----------------------------------------------------------------------

#region

using System.Collections.Generic;
using Core.Entities;

#endregion

namespace Core.GlobalRepository
{
    public interface IReconoserRepository
    {
        string GetPasswordEncryptedAccount(string user, string password, long account);

        void SaveData(string usuario, string password, List<Balance> Saldos);
        List<Transaction> GetData(string user, string password);
    }
}
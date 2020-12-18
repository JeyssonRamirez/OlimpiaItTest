using System;
using System.Collections.Generic;
using Core.Entities;

namespace Application.Definition
{
    public interface IAccountService
    {
        void ProcessArray(List<Transaction> data, ref CompletedData referenceData, Delegate call);
        void ProcessCompleted(int divider);
        void SaveData(string user, string password, List<Balance> balances);
    }
}

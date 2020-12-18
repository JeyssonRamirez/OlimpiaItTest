using System;
using System.Collections.Generic;
using Core.Entities;

namespace Application.Definition
{
    public delegate void UpdateStatusEventHandler(decimal status);
    public interface IAccountService
    {
        void ProcessArray(List<Transaction> data, ref CompletedData referenceData, UpdateStatusEventHandler call);
        void ProcessCompleted(int divider, List<Transaction> data);
        void SaveData(string user, string password, List<Balance> balances);
        List<Transaction> GetData();

        void CancelProcess();
    }
}

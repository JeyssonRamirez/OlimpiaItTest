using System;
using System.Collections.Generic;
using Core.Entities;

namespace Application.Definition
{
    public delegate void UpdateStatusEventHandler(decimal status,string error);
    public interface IAccountService
    {
        void ProcessArray(List<Transaction> data,  CompletedData referenceData, UpdateStatusEventHandler call);

        void ProcessCompleted(int divider, CompletedData referenceData, List<Transaction> resp,
            UpdateStatusEventHandler call);
        void SaveData(string user, string password, List<Balance> balances);
        List<Transaction> GetData();

        void CancelProcess();
    }
}

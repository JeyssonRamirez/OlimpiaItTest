using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Definition;
using Core.Entities;
using Core.GlobalRepository;

namespace Application.Implementation
{
    public class AccountService : IAccountService
    {
        private IReconoserRepository _reconoserRepository;
        private List<Thread> _thread;

        public AccountService(IReconoserRepository reconoserRepository)
        {
            _reconoserRepository = reconoserRepository;
        }

        private string DecriptData(string claveCifrada, string Cadena)
        {
            //Este metodo no se requiere estructurar / optimizar
            byte[] Clave = Encoding.ASCII.GetBytes(claveCifrada);
            byte[] IV = Encoding.ASCII.GetBytes("1234567812345678");


            byte[] inputBytes = Convert.FromBase64String(Cadena);
            string textoLimpio = String.Empty;
            RijndaelManaged cripto = new RijndaelManaged();
            using (MemoryStream ms = new MemoryStream(inputBytes))
            {
                using (CryptoStream objCryptoStream =
                    new CryptoStream(ms, cripto.CreateDecryptor(Clave, IV), CryptoStreamMode.Read))
                {
                    using (StreamReader sr = new StreamReader(objCryptoStream, true))
                    {
                        textoLimpio = sr.ReadToEnd();
                    }
                }
            }

            return textoLimpio;
        }
        private long CalculateComision(long n)
        {
            var count = 0;
            long a = 0;
            while (count < n)
            {
                a = 2;

                long b = 2;
                int prime = 1;
                while (b * b <= a)
                {
                    if (a % b == 0)
                    {
                        prime = 0;
                        break;
                    }

                    b++;
                }

                if (prime > 0)
                {
                    count++;
                }

                a++;
            }

            return (--a);
        }
        public void ProcessArray(List<Transaction> data, CompletedData referenceData, UpdateStatusEventHandler call)
        {
            try
            {
                foreach (var currentTransaction in data)
                {
                    var currentAccount = currentTransaction.CuentaOrigen;
                    var password = "";
                    var current = referenceData.ProcessedAccounts.FirstOrDefault(s => s.AccountNumber == currentAccount);
                    if (current == null)
                    {
                        password = _reconoserRepository.GetPasswordEncryptedAccount("usuariop", "passwordp", currentAccount);
                        referenceData.ProcessedAccounts.Add(new MyAccount
                        {
                            AccountNumber = currentAccount,
                            Password = password
                        });
                    }
                    else
                    {
                        password = current.Password;
                    }

                    var currentMovement = DecriptData(password, currentTransaction.TipoTransaccion);

                    double saldoActual = -1;

                    //Obtenemos el saldo actual de la cuenta
                    foreach (var balance in referenceData.BalanceList)
                    {
                        if (currentAccount != balance.CuentaOrigen) continue;
                        saldoActual = balance.SaldoCuenta;

                        double commission = CalculateComision(Convert.ToInt64(currentTransaction.ValorTransaccion));

                        if (currentMovement == "Debito")
                        {
                            balance.SaldoCuenta -= currentTransaction.ValorTransaccion;
                        }
                        else
                        {
                            balance.SaldoCuenta += currentTransaction.ValorTransaccion - commission;
                        }
                    }

                    //Si no encuentra lo inserta
                    if (saldoActual == -1)
                    {
                        var newBalance = new Balance();


                        double comision = CalculateComision(Convert.ToInt64(currentTransaction.ValorTransaccion));

                        newBalance.CuentaOrigen = currentAccount;
                        if (currentMovement == "Debito")
                        {
                            newBalance.SaldoCuenta -= currentTransaction.ValorTransaccion;
                        }
                        else
                        {
                            newBalance.SaldoCuenta += currentTransaction.ValorTransaccion - comision;
                        }

                        referenceData.BalanceList.Add(newBalance);
                    }

                    decimal percentage = 100 / referenceData.Total * referenceData.Counter;
                    referenceData.Counter++;
                    call(Math.Round(percentage, 2),"");
                }
            }
            catch (ThreadAbortException e)
            {

                call(0, e.Message);
            }

            catch (Exception e)
            {
                call(0, e.Message);
            }
        }
        
        public void SaveData(string usuariop, string passwordp, List<Balance> balances)
        {
            _reconoserRepository.SaveData(usuariop, usuariop, balances);
        }

        public List<Transaction> GetData()
        {
            return _reconoserRepository.GetData("usuariop", "passwordp");
        }

        public void CancelProcess()
        {
            if (_thread != null)
            {
                var counter = 1;
                foreach (var thread in _thread)
                {
                    counter++;
                    thread.Abort($"Proceso {counter} detenido");
                }
                _thread = null;
            }
        }


        public void ProcessCompleted(int divider,  CompletedData referenceData, List<Transaction> resp, UpdateStatusEventHandler call)
        {
            divider = divider == 0 ? 1 : divider;
            referenceData.Total = resp.Count;
            referenceData.BalanceList = new List<Balance>();
            List<List<Transaction>> group = new List<List<Transaction>>();
            if (divider == 1)
            {
                group = new List<List<Transaction>> { resp };
            }
            else
            {
                divider = divider - 1;

                var countByDivide = resp.Count / divider;

                int startIndex = 0;
                while (startIndex < resp.Count)
                {
                    var item = countByDivide;
                    if (startIndex + item > resp.Count)
                    {
                        item = (startIndex + item) - resp.Count;
                    }

                    group.Add(resp.Skip(startIndex).Take(item).ToList());
                    //processArray(group, startIndex, group.First().ThreadCount);
                    startIndex += countByDivide;
                }
                //group = Split(resp, divider);
            }

         



            _thread = new List<Thread>();
            var countert = 1;
            foreach (var array in group)
            {
                //Task task = Task.Factory.StartNew(() => ProcessArray(array));
                //tasks.Add(task);
                Thread myNewThread = new Thread(() => ProcessArray(array, referenceData, call));
                myNewThread.Priority = ThreadPriority.Highest;
                myNewThread.Name = "Thread" + countert;
                _thread.Add(myNewThread);
            }

            foreach (var current in _thread)
            {
                current.Start();
            }
        }
    }
}

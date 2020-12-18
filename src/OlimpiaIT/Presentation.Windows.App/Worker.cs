//   -----------------------------------------------------------------------
//   <copyright file=Worker.cs company="Jeysson Ramirez">
//       Copyright (c) Jeysson Ramirez Todos los derechos reservados.
//   </copyright>
//   <author>Jeysson Stevens  Ramirez </author>
//   <Date>  2020 -12-17  - 17:43</Date>
//   <Update> 2020-12-17 - 18:22</Update>
//   -----------------------------------------------------------------------

#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;


#endregion

namespace Presentation.Windows.App
{
    //public class Worker
    //{
    //    public delegate void Delegate(decimal percentage);

    //    private readonly ServiceClient _client;

    //    public Worker(ServiceClient client)
    //    {
    //        _client = client;
    //    }

    //    public string DecriptData(string claveCifrada, string Cadena)
    //    {
    //        //Este metodo no se requiere estructurar / optimizar
    //        byte[] Clave = Encoding.ASCII.GetBytes(claveCifrada);
    //        byte[] IV = Encoding.ASCII.GetBytes("1234567812345678");


    //        byte[] inputBytes = Convert.FromBase64String(Cadena);
    //        string textoLimpio = String.Empty;
    //        RijndaelManaged cripto = new RijndaelManaged();
    //        using (MemoryStream ms = new MemoryStream(inputBytes))
    //        {
    //            using (CryptoStream objCryptoStream =
    //                new CryptoStream(ms, cripto.CreateDecryptor(Clave, IV), CryptoStreamMode.Read))
    //            {
    //                using (StreamReader sr = new StreamReader(objCryptoStream, true))
    //                {
    //                    textoLimpio = sr.ReadToEnd();
    //                }
    //            }
    //        }

    //        return textoLimpio;
    //    }

    //    public long CalculateComision(long n)
    //    {
    //        var count = 0;
    //        long a = 0;
    //        while (count < n)
    //        {
    //            a = 2;

    //            long b = 2;
    //            int prime = 1;
    //            while (b * b <= a)
    //            {
    //                if (a % b == 0)
    //                {
    //                    prime = 0;
    //                    break;
    //                }

    //                b++;
    //            }

    //            if (prime > 0)
    //            {
    //                count++;
    //            }

    //            a++;
    //        }

    //        return (--a);
    //    }

    //    public void ProcessArray(IEnumerable<Transaccion> data, ref CompletedData referenceData, Delegate call)
    //    {
    //        try
    //        {
    //            foreach (var currentTransaction in data)
    //            {
    //                var currentAccount = currentTransaction.CuentaOrigen;
    //                var password = "";
    //                var current = referenceData.ProcessedAccounts.FirstOrDefault(s => s.AccountNumber == currentAccount);
    //                if (current == null)
    //                {
    //                    password = _client.GetClaveCifradoCuenta("usuariop", "passwordp", currentAccount);
    //                    referenceData.ProcessedAccounts.Add( new MyAccount
    //                    {
    //                        AccountNumber = currentAccount,
    //                        Password =password
    //                    });
    //                }
    //                else
    //                {
    //                    password = current.Password;
    //                }
                    
    //                var currentMovement = DecriptData(password, currentTransaction.TipoTransaccion);

    //                double saldoActual = -1;

    //                //Obtenemos el saldo actual de la cuenta
    //                foreach (var balance in referenceData.BalanceList)
    //                {
    //                    if (currentAccount != balance.CuentaOrigen) continue;
    //                    saldoActual = balance.SaldoCuenta;

    //                    double commission = CalculateComision(Convert.ToInt64(currentTransaction.ValorTransaccion));

    //                    if (currentMovement == "Debito")
    //                    {
    //                        balance.SaldoCuenta -= currentTransaction.ValorTransaccion;
    //                    }
    //                    else
    //                    {
    //                        balance.SaldoCuenta += currentTransaction.ValorTransaccion - commission;
    //                    }
    //                }

    //                //Si no encuentra lo inserta
    //                if (saldoActual == -1)
    //                {
    //                    var newBalance = new Saldo();


    //                    double comision = CalculateComision(Convert.ToInt64(currentTransaction.ValorTransaccion));

    //                    newBalance.CuentaOrigen = currentAccount;
    //                    if (currentMovement == "Debito")
    //                    {
    //                        newBalance.SaldoCuenta -= currentTransaction.ValorTransaccion;
    //                    }
    //                    else
    //                    {
    //                        newBalance.SaldoCuenta += currentTransaction.ValorTransaccion - comision;
    //                    }

    //                    referenceData.BalanceList.Add(newBalance);
    //                }

    //                decimal percentage = 100 / referenceData.Total * referenceData.Counter;
    //                referenceData.Counter++;
    //                call(Math.Round(percentage, 2));
    //            }
    //        }
    //        catch (ThreadAbortException)
    //        {
                
    //            MessageBox.Show("Proceso Detenido", "", MessageBoxButtons.OK);
    //            //throw abortException;
    //        }

    //        catch (Exception e)
    //        {
    //            MessageBox.Show(e.InnerException != null ? e.InnerException.Message : e.Message, "",
    //                MessageBoxButtons.OK);
    //        }
    //    }
 //   }
}
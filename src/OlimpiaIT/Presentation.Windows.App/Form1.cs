//PRUEBA OLIMPIA
//La función de la aplicación actual es calcular el saldo final de las cuentas de un "banco", para esto se consume un servicio que devuelve 
//las transacciones realizas a la cuentas.

//Paso 1: Hacer funcionar la aplicación. Debido al aumento de transacciones y al  colocar al servicio con SSL la aplicación actual esta fallando.
//Paso 2: Estructurar mejor el codigo. Uso de patrones, buenas practicas, etc.
//Paso 3: Optimizar el codigo, como se menciono en el paso 1 el aumento de transacciones ha causado que el calculo de los saldos se demore demasiado.
//Paso 4: Adicionar una barra de progreso al formulario. Actualizar la barra con el progreso del proceso, evitando bloqueos del GUI.


using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Forms;

namespace Presentation.Windows.App
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnCalcular_Click(object sender, EventArgs e)
        {            

            System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();


            //Trust all certificates
            System.Net.ServicePointManager.ServerCertificateValidationCallback = ((senderOne, certificate, chain, sslPolicyErrors) => true);

            // trust sender
            //System.Net.ServicePointManager.ServerCertificateValidationCallback = ((senderOne, cert, chain, errors) => cert.Subject.Contains("YourServerName"));

            ServicioPrueba.ServiceClient client = new ServicioPrueba.ServiceClient();


            var resp = client.GetData("usuariop", "passwordp");

            //Variable donse se almacenan los saldos finales
            var balanceList = new List<ServicioPrueba.Saldo>();

            foreach (var currentTransaction in resp)
            {
                var cuentaActual = currentTransaction.CuentaOrigen;
                var claveCifrado = client.GetClaveCifradoCuenta("usuariop", "passwordp", cuentaActual);
                var movimientoActual = Desencripta(claveCifrado, currentTransaction.TipoTransaccion);

                double saldoActual = -1;

                //Obtenemos el saldo actual de la cuenta
                foreach (var saldo  in balanceList)
                {
                    if (cuentaActual == saldo.CuentaOrigen)
                    {
                        saldoActual = saldo.SaldoCuenta;

                        double comision = CalcularComision(Convert.ToInt64(currentTransaction.ValorTransaccion));

                        if (movimientoActual == "Debito")
                        {
                            saldo.SaldoCuenta -= currentTransaction.ValorTransaccion;
                        }
                        else
                        {
                            saldo.SaldoCuenta += currentTransaction.ValorTransaccion - comision;
                        }
                    }
                }

                //Si no encuentra lo inserta
                if(saldoActual == -1)
                {
                    var newBalance = new ServicioPrueba.Saldo();


                    double comision = CalcularComision(Convert.ToInt64(currentTransaction.ValorTransaccion));

                    newBalance.CuentaOrigen = cuentaActual;                   
                    if (movimientoActual == "Debito")
                    {
                        newBalance.SaldoCuenta -= currentTransaction.ValorTransaccion;
                    }
                    else
                    {
                        newBalance.SaldoCuenta += currentTransaction.ValorTransaccion - comision; 
                    }

                    balanceList.Add(newBalance);
                } 
            }


            sw.Stop();
            lblTiempoTotal.Text = sw.ElapsedMilliseconds.ToString();

            //Enviamos los saldos finales
            client.SaveData("usuariop", "passwordp", balanceList.ToArray());


           
        }

        public long CalcularComision(long n)
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

        public string Desencripta(string ClaveCifrado, string Cadena)
        {
            //Este metodo no se requiere estructurar / optimizar
            byte[] Clave = Encoding.ASCII.GetBytes(ClaveCifrado);
            byte[] IV = Encoding.ASCII.GetBytes("1234567812345678");
            

            byte[] inputBytes = Convert.FromBase64String(Cadena);
            string textoLimpio = String.Empty;
            RijndaelManaged cripto = new RijndaelManaged();
            using (MemoryStream ms = new MemoryStream(inputBytes))
            {
                using (CryptoStream objCryptoStream = new CryptoStream(ms, cripto.CreateDecryptor(Clave, IV), CryptoStreamMode.Read))
                {
                    using (StreamReader sr = new StreamReader(objCryptoStream, true))
                    {
                        textoLimpio = sr.ReadToEnd();
                    }
                }
            }
            return textoLimpio;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}

//   -----------------------------------------------------------------------
//   <copyright file=Form1.cs company="Jeysson Ramirez">
//       Copyright (c) Jeysson Ramirez Todos los derechos reservados.
//   </copyright>
//   <author>Jeysson Stevens  Ramirez </author>
//   <Date>  2020 -12-17  - 11:55</Date>
//   <Update> 2020-12-17 - 15:00</Update>
//   -----------------------------------------------------------------------

//Paso 1: Hacer funcionar la aplicación. Debido al aumento de transacciones y al  colocar al servicio con SSL la aplicación actual esta fallando.
//Paso 2: Estructurar mejor el codigo. Uso de patrones, buenas practicas, etc.
//Paso 3: Optimizar el codigo, como se menciono en el paso 1 el aumento de transacciones ha causado que el calculo de los saldos se demore demasiado.
//Paso 4: Adicionar una barra de progreso al formulario. Actualizar la barra con el progreso del proceso, evitando bloqueos del GUI.


#region

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Application.Definition;
using Core.Entities;
using Crosscutting.DependencyInjectionFactory;

#endregion

namespace Presentation.Windows.App
{
    public partial class Form1 : Form
    {
        private readonly IAccountService _accountService;
        public CompletedData CompletedData;


        private List<Thread> _thread;
        private List<Task> _tasks;


        private Stopwatch sw;
        public Form1()
        {
            
            InitializeComponent();
            _accountService = Factory.Resolve<IAccountService>();
            //Trust all certificates
        }

        // On worker thread so do our thing!
        delegate void SetTextCallback(string text);
        delegate void SetPercentageCallback(int percentage);

        private void SetTextLabelStatus(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.labelStatus.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetTextLabelStatus);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.labelStatus.Text = text;
            }
        }
        private void SetTimeTotal(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.labelStatus.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetTimeTotal);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.lblTiempoTotal.Text = text;
            }
        }
        private void SetPercentage(int percentage)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.labelStatus.InvokeRequired)
            {
                SetPercentageCallback d = new SetPercentageCallback(SetPercentage);
                this.Invoke(d, new object[] { percentage });
            }
            else
            {
                progressBar.Value = percentage;
            }
        }
        private void SetVisibleByNewMethodInvoker(decimal percentage)
        {

            
            if (percentage == 100)
            {

                _accountService.SaveData("usuariop", "passwordp", CompletedData.BalanceList);

                //Client.SaveData("usuariop", "passwordp", CompletedData.BalanceList);
                sw.Stop();
                //btnCalcular.Enabled = true;
                SetTextLabelStatus("Completado");
                MessageBox.Show("Termino !!!", "", MessageBoxButtons.OK);
            }
            else
            {

                switch (labelStatus.Text)
                {
                    case "Procesando":
                        SetTextLabelStatus($"{percentage} Procesando.");
                        break;
                    case "Procesando.":
                        SetTextLabelStatus($"{percentage} Procesando..");
                        break;
                    case "Procesando..":
                        SetTextLabelStatus($"{percentage} Procesando...");
                        break;
                    default:
                        SetTextLabelStatus($"{percentage} Procesando");
                        break;
                }

            }
            SetTimeTotal(sw.ElapsedMilliseconds.ToString());
            SetPercentage((int)Math.Round(percentage, 0));
        }
        private void btnCalcular_Click(object sender, EventArgs e)
        {

            try
            {
                if (texThreath.Text == "")
                {
                    MessageBox.Show("Por Favor digite el numero de hilos a usar del PC", "", MessageBoxButtons.OK);
                }
                else
                {
                    _accountService.ProcessCompleted(int.Parse(texThreath.Text));
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.InnerException != null ? exception.InnerException.Message : exception.Message, "", MessageBoxButtons.OK);
            }
            

            //try
            //{
            //    CompletedData = new CompletedData();
            //    CompletedData.Counter = 1;
            //    CompletedData.ProcessedAccounts = new List<MyAccount>();

            //    var divider = 1;
            //    if (texThreath.Text == "")
            //    {
            //        MessageBox.Show("Por Favor digite el numero de hilos a usar del PC", "", MessageBoxButtons.OK);
            //    }
            //    else
            //    {

            //        divider = int.Parse(texThreath.Text);

            //        divider = divider == 0 ? 1 : divider;

            //        btnCalcular.Enabled = false;
            //        progressBar.Value = 0;


            //        labelStatus.Text = "Consultando Trabajo";

            //        sw = Stopwatch.StartNew();
            //        Transaccion[] resp = Client.GetData("usuariop", "passwordp");
            //        CompletedData.Total = resp.Length;

            //        CompletedData.BalanceList = new List<Balance>();
            //        List<IEnumerable<Transaccion>> group= new List<IEnumerable<Transaccion>>();
            //        if (divider == 1)
            //        {
            //            group = new List<IEnumerable<Transaccion>>
            //            {
            //                resp
            //            };
            //        }
            //        else
            //        {
            //            //divider = divider - 1;

            //            var countByDivide = resp.Length / divider;

            //            int startIndex = 0;
            //            while (startIndex < resp.Length)
            //            {
            //                var item = countByDivide;
            //                if (startIndex + item > resp.Length)
            //                {
            //                    item = (startIndex + item) - resp.Length;
            //                }

            //                group.Add(resp.Skip(startIndex).Take(item).ToList());
            //                //processArray(group, startIndex, group.First().ThreadCount);
            //                startIndex += countByDivide;
            //            }
            //            //group = Split(resp, divider);
            //        }


            //        _tasks = new List<Task>();
            //        _thread = new List<Thread>();
            //        var countert = 1;
            //        foreach (var array in group)
            //        {
            //            //Task task = Task.Factory.StartNew(() => ProcessArray(array));
            //            //tasks.Add(task);

            //            _accountService.ProcessArray(array,);
            //            var worker = new Worker(Client);

            //            Thread myNewThread = new Thread(() => worker.ProcessArray(array, ref this.CompletedData, SetVisibleByNewMethodInvoker));
            //            myNewThread.Priority = ThreadPriority.Highest;
            //            myNewThread.Name = "Thread" + countert;
            //            _thread.Add(myNewThread);
            //        }

            //        btnStop.Enabled = true;

            //        labelStatus.Text = "Procesando";
            //        foreach (var current in _thread)
            //        {
            //            current.Start();
            //        }
            //    }
            //}
            //catch (Exception exception)
            //{

            //    
            //}
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
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

            CompletedData.Counter = 1;
            MessageBox.Show("Stop...", "", MessageBoxButtons.OK);
            btnCalcular.Enabled = true;
            btnStop.Enabled = false;
            labelStatus.Text = "Sin Iniciar";
            progressBar.Value = 0;
            sw.Stop();
        }
    }
}
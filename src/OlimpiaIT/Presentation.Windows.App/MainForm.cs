//   -----------------------------------------------------------------------
//   <copyright file=Form1.cs company="Jeysson Ramirez">
//       Copyright (c) Jeysson Ramirez Todos los derechos reservados.
//   </copyright>
//   <author>Jeysson Stevens  Ramirez </author>
//   <Date>  2020 -12-17  - 11:55</Date>
//   <Update> 2020-12-17 - 15:00</Update>
//   -----------------------------------------------------------------------

//Paso 1: Hacer funcionar la aplicación. Debido al aumento de transacciones y al  colocar al servicio con SSL la aplicación actual esta fallando. OK 
//Paso 2: Estructurar mejor el codigo. Uso de patrones, buenas practicas, etc. OK
//Paso 3: Optimizar el codigo, como se menciono en el paso 1 el aumento de transacciones ha causado que el calculo de los saldos se demore demasiado. OK 
//Paso 4: Adicionar una barra de progreso al formulario. Actualizar la barra con el progreso del proceso, evitando bloqueos del GUI. 


#region

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Application.Definition;
using Core.Entities;
using Crosscutting.DependencyInjectionFactory;

#endregion

namespace Presentation.Windows.App
{
    public partial class MainForm : Form
    {
        private readonly IAccountService _accountService;
        public CompletedData CompletedData;
        private Stopwatch sw;
        public MainForm()
        {

            InitializeComponent();
            _accountService = Factory.Resolve<IAccountService>();


            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;

        }

        // On worker thread so do our thing!
        delegate void SetTextCallback(string text);
        delegate void SetPercentageCallback(int percentage);
        delegate void SetEnableeCallback(bool option);

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
        private void SetVisibleByNewMethodInvoker(decimal percentage, string message)
        {


            if (!string.IsNullOrWhiteSpace(message))
            {
                MessageBox.Show(message, "", MessageBoxButtons.OK);
            }
            else
            {
                if (percentage == 100)
                {

                    _accountService.SaveData("usuariop", "passwordp", CompletedData.BalanceList);

                    sw.Stop();
                    //btnCalcular.Enabled = true;
                    //btnStop.Enabled = false;
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


                    btnCalcular.Enabled = false;
                    progressBar.Value = 0;
                    labelStatus.Text = "Consultando Trabajo";
                    sw = Stopwatch.StartNew();


                    btnStop.Enabled = true;
                    labelStatus.Text = "Procesando";

                    var data = _accountService.GetData();
                    CompletedData = new CompletedData
                    {
                        BalanceList = new List<Balance>(),
                        Counter = 1,
                        ProcessedAccounts = new List<MyAccount>(),
                        Total = data.Count
                    };
                    _accountService.ProcessCompleted(int.Parse(texThreath.Text), CompletedData, data, SetVisibleByNewMethodInvoker);


                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.InnerException != null ? exception.InnerException.Message : exception.Message, "", MessageBoxButtons.OK);
            }
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
        // This event handler updates the progress.



        private void btnStop_Click(object sender, EventArgs e)
        {


            //if (backgroundWorker1.WorkerSupportsCancellation == true)
            //{
            //    // Cancel the asynchronous operation.
            //    backgroundWorker1.CancelAsync();
            //}

            _accountService.CancelProcess();
            CompletedData.Counter = 1;
            MessageBox.Show("Stop...", "", MessageBoxButtons.OK);
            btnCalcular.Enabled = true;
            btnStop.Enabled = false;
            labelStatus.Text = "Sin Iniciar";
            progressBar.Value = 0;
            sw.Stop();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            if (worker.CancellationPending == true)
            {
                e.Cancel = true;
            }
            // Perform a time consuming operation and report progress.



        }
    }
}
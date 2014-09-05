using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArduinoSerialControler
{
    public partial class MainForm : Form
    {
        string data;

        public MainForm()
        {
            InitializeComponent();
            StartButton.Enabled = false;
            StopButton.Enabled = false;
            SendButton.Enabled = false;
            SendTextBox.Enabled = false;
            string[] ports = System.IO.Ports.SerialPort.GetPortNames();
            if (ports.Count() != 0)
            {
                StartButton.Enabled = true;
                foreach (string port in ports)
                    PortComboBox.Items.Add(port);
                PortComboBox.SelectedIndex = 0;
            }
            else
                EditStatusLabel.Text = "Brak portów COM";

        }

        private void DisplayData(object sender, EventArgs e)
        {
            MainTextBox.AppendText(data);
            MainTextBox.AppendText(Environment.NewLine);
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            serialPort.Close();
        }

        private void MainTextBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            MainTextBox.Clear();
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            string data = SendTextBox.Text;
            serialPort.WriteLine(data);
            SendTextBox.Clear();
        }

        private void SendTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                this.Invoke(new EventHandler(SendButton_Click));
            }
        }

        private void serialPort_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            data = serialPort.ReadLine();
            this.Invoke(new EventHandler(DisplayData));
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            if(!serialPort.IsOpen)
            {
                try
                {
                    serialPort.PortName = PortComboBox.Text;
                    serialPort.BaudRate = 9600;
                    serialPort.Open();

                    EditStatusLabel.Text = "Otwarto port " + serialPort.PortName;

                    StartButton.Enabled = false;
                    SendButton.Enabled = true;
                    StopButton.Enabled = true;
                    SendTextBox.Enabled = true;
                }
                catch (Exception)
                {
                    EditStatusLabel.Text = "Nie można otworzyć portu " + PortComboBox.Text;
                }
            }
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            serialPort.Close();
            EditStatusLabel.Text = "Zamknieto port " + serialPort.PortName;

            StartButton.Enabled = true; 
            StopButton.Enabled = false;
            SendButton.Enabled = false;
        }
          
        private void label1_MouseClick(object sender, MouseEventArgs e)
        {
            MessageBox.Show("ArduinoSerialControler " + Application.ProductVersion.Substring(0,5) + "\nWykonał crowmw \n\nKomunikacja z Arduino poprzez port COM. \n\nAby komunikować się z Arduino należy wyłączyć monitor portu szeregowego w IDE\n\nDouble-click czyści okno", "Halp!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}

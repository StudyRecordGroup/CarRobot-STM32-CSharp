using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace BluetoothPairTool
{
    public partial class Form2 : Form
    {
        Dictionary<string, decimal> Parity = new Dictionary<string, decimal>()
        {
            { "None", 0},
            { "Odd", 1},
            { "Even", 2},
            { "Mark", 3},
        };

        Dictionary<string, int> StopBit = new Dictionary<string, int>()
        {
            { "None", 0},
            { "One", 1},
            { "Two", 2},
            { "OnePointFive", 3},
        };

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();
            comboBox_PortNum.DataSource = ports;
            comboBox_BaudRate.SelectedIndex = 11;
            comboBox_DataBits.SelectedIndex = 0;
            comboBox_Parity.DataSource = new BindingSource(Parity, null);
            comboBox_Parity.DisplayMember = "Key";
            comboBox_Parity.ValueMember = "Value";
            comboBox_StopBit.DataSource = new BindingSource(StopBit, null);
            comboBox_StopBit.DisplayMember = "Key";
            comboBox_StopBit.ValueMember = "Value";
            comboBox_StopBit.SelectedIndex = 1;
        }

        private void button_refresh_Click(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();
            comboBox_PortNum.DataSource = ports;
        }

        private void button_Open_Click(object sender, EventArgs e)
        {
            SerialPort serialPort = new SerialPort(comboBox_PortNum.Text, int.Parse(comboBox_BaudRate.Text),
                (Parity)Parity[comboBox_Parity.Text], int.Parse(comboBox_DataBits.Text), (StopBits)StopBit[comboBox_StopBit.Text]);
            serialPort.Open();
            if(serialPort.IsOpen)
            {
                using (var dlg = new Form_RemoteCar(serialPort))
                {
                    dlg.ShowDialog();
                }
            }
        }
    }
}

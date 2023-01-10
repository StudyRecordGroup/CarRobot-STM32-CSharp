using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Newtonsoft.Json;
using System.IO;
using System.IO.Ports;

namespace BluetoothPairTool
{

    public partial class Form_RemoteCar : Form
    {
        SerialPort serialPort;
        CancellationTokenSource m_cancel = new CancellationTokenSource();

        enum MoveCmd
        {
            Stop,
            Front,
            Left,
            Right,
            Back
        }

        public Form_RemoteCar(SerialPort socket)
        {
            InitializeComponent();
            serialPort = socket;
        }

        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        public static string RemoveWhitespace(string input)
        {
            return new string(input.ToCharArray()
                .Where(c => !Char.IsWhiteSpace(c))
                .ToArray());
        }

        private void Form_RemoteCar_Load(object sender, EventArgs e)
        {

            //ReceiveStringLoop(chatReader);
            BackgroundWorker worker = new BackgroundWorker();
            worker.RunWorkerAsync(serialPort);
            button_Front.KeyDown += button_KeyDown;
            button_Back.KeyDown += button_KeyDown;
            button_Left.KeyDown += button_KeyDown;
            button_Right.KeyDown += button_KeyDown;
            button_Front.KeyUp += button_KeyUp;
            button_Back.KeyUp += button_KeyUp;
            button_Left.KeyUp += button_KeyUp;
            button_Right.KeyUp += button_KeyUp;
        }

        private delegate void ShowMessage(string sMessage);
        private void UpdateResponse(string msg)
        {
            if (InvokeRequired)
            {
                ShowMessage op = new ShowMessage(UpdateResponse);
                Invoke(op, msg);
            }
            else
            {
                listBox1.Items.Add(msg);
            }
        }

        private void Form_RemoteCar_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_cancel.Cancel();
            serialPort.Close();
            serialPort.Dispose();
        }

        private async void button_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W || e.KeyCode == Keys.Up)
                await sendCmd((byte)MoveCmd.Front);
            if (e.KeyCode == Keys.A || e.KeyCode == Keys.Left)
                await sendCmd((byte)MoveCmd.Left);
            if (e.KeyCode == Keys.D || e.KeyCode == Keys.Right)
                await sendCmd((byte)MoveCmd.Right);
            if (e.KeyCode == Keys.S || e.KeyCode == Keys.Down)
                await sendCmd((byte)MoveCmd.Back);
        }

        private async void button_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W || e.KeyCode == Keys.Up)
                await sendCmd((byte)MoveCmd.Stop);
            if (e.KeyCode == Keys.A || e.KeyCode == Keys.Left)
                await sendCmd((byte)MoveCmd.Stop);
            if (e.KeyCode == Keys.D || e.KeyCode == Keys.Right)
                await sendCmd((byte)MoveCmd.Stop);
            if (e.KeyCode == Keys.S || e.KeyCode == Keys.Down)
                await sendCmd((byte)MoveCmd.Stop);
        }

        private async Task sendCmd(byte cmd)
        {
            try
            {
                byte[] cmds = new byte[] { cmd };
                Debug.WriteLine($">> { BitConverter.ToString(cmds).Replace("-", " ")}");
                serialPort.Write(cmds, 0, 1);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {

            }
        }
    }
}

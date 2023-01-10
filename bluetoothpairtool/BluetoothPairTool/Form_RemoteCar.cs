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
        enum DirKey
        {
            None,
            Up,
            Left,
            Right,
            Down,
        }

        DirKey pressKey;
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
            pressKey = DirKey.None;
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
            this.Focus();
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

        private async Task sendCmd(byte cmd)
        {
            try
            {
                byte[] cmds = new byte[] { cmd };
                string msg = $">> { BitConverter.ToString(cmds).Replace("-", " ")}";
                Debug.WriteLine(msg);
                UpdateResponse(msg);
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

        private void Form_RemoteCar_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.W ||
                e.KeyCode == Keys.A ||
                e.KeyCode == Keys.D ||
                e.KeyCode == Keys.S )
            {
                e.IsInputKey = true;
            }
        }

        private async void Form_RemoteCar_KeyDown(object sender, KeyEventArgs e)
        {
            if (pressKey != DirKey.None) return;
            if (e.KeyCode == Keys.W)
            {
                pressKey = DirKey.Up;
                button_Front.BackColor = Color.LawnGreen;
                await sendCmd((byte)MoveCmd.Front);
            }
            if (e.KeyCode == Keys.A)
            {
                pressKey = DirKey.Left;
                button_Left.BackColor = Color.LawnGreen;
                await sendCmd((byte)MoveCmd.Left);
            }
            if (e.KeyCode == Keys.D)
            {
                pressKey = DirKey.Right;
                button_Right.BackColor = Color.LawnGreen;
                await sendCmd((byte)MoveCmd.Right);
            }
            if (e.KeyCode == Keys.S)
            {
                pressKey = DirKey.Down;
                button_Back.BackColor = Color.LawnGreen;
                await sendCmd((byte)MoveCmd.Back);
            }
        }

        private async void Form_RemoteCar_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W)
            {
                await sendCmd((byte)MoveCmd.Stop);
                button_Front.BackColor = Color.Transparent;
            }
            if (e.KeyCode == Keys.A)
            {
                await sendCmd((byte)MoveCmd.Stop);
                button_Left.BackColor = Color.Transparent;
            }
            if (e.KeyCode == Keys.D)
            {
                await sendCmd((byte)MoveCmd.Stop);
                button_Right.BackColor = Color.Transparent;
            }
            if (e.KeyCode == Keys.S)
            {
                await sendCmd((byte)MoveCmd.Stop);
                button_Back.BackColor = Color.Transparent;
            }
            pressKey = DirKey.None;
        }
    }
}

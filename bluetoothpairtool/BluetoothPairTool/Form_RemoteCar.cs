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

namespace BluetoothPairTool
{

    public partial class Form_RemoteCar : Form
    {
        StreamSocket m_socket;
        DataWriter m_chatWriter;
        DataReader m_chatReader;
        CancellationTokenSource m_cancel = new CancellationTokenSource();

        enum MoveCmd
        {
            Stop,
            Front,
            Left,
            Right,
            Back
        }

        public Form_RemoteCar(StreamSocket socket)
        {
            InitializeComponent();
            m_socket = socket;
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
            m_chatReader = new DataReader(m_socket.InputStream);
            m_chatWriter = new DataWriter(m_socket.OutputStream);
            //ReceiveStringLoop(chatReader);
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += Worker_DoWork;
            worker.RunWorkerAsync(m_chatReader);
            button_Front.KeyDown += button_KeyDown;
            button_Back.KeyDown += button_KeyDown;
            button_Left.KeyDown += button_KeyDown;
            button_Right.KeyDown += button_KeyDown;
            button_Front.KeyUp += button_KeyUp;
            button_Back.KeyUp += button_KeyUp;
            button_Left.KeyUp += button_KeyUp;
            button_Right.KeyUp += button_KeyUp;
        }

        private async void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorker;
            var chatReader = e.Argument as DataReader;
            byte[] data = new byte[1];
            try
            {
                while (!m_cancel.IsCancellationRequested)
                {
                    uint size = await chatReader.LoadAsync(1);
                    if (size != 1)
                    {
                        Debug.WriteLine("Remote device terminated connecton");
                        return;
                    }
                    chatReader.ReadBytes(data);
                    var rsp = $"{BitConverter.ToString(data).Replace('-', ' ')} ";
                    Debug.WriteLine($"<< {rsp}");
                    UpdateResponse(rsp);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                Debug.WriteLine("Worker terminated");
            }
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
            m_chatWriter.Dispose();
            m_chatReader.Dispose();
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
                Debug.WriteLine($">> { cmd.ToString("x")}");
                m_chatWriter.WriteByte(cmd);
                await m_chatWriter.StoreAsync();
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

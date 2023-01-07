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
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Rfcomm;
using Windows.Devices.Enumeration;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace BluetoothPairTool
{
    public partial class Form1 : Form
    {
        private DeviceWatcher watcher = null;
        private Dictionary<string, ListViewItem> m_listviews;
        public Form1()
        {
            InitializeComponent();
            button_Pair.Enabled = button_Unpair.Enabled = button_stop.Enabled = false;
            button_SPP.Enabled = false;
            columnHeader_Id.Width = -2;
            
        }

        private void button_start_Click(object sender, EventArgs e)
        {
            if (watcher != null) return;
            var Selector = "System.Devices.Aep.ProtocolId:=\"{e0cbf06c-cd8b-4647-bb8a-263b43f0f974}\"";
            //string selector = "(" + Selector + ")" + " AND (System.Devices.Aep.CanPair:=System.StructuredQueryType.Boolean#True OR System.Devices.Aep.IsPaired:=System.StructuredQueryType.Boolean#True)";
            string selector = "(" + Selector + ")";
            List<string> additionalProperties = new List<string>()
            {
                "System.Devices.Aep.IsConnected",
                "System.Devices.Aep.Manufacturer",
                "System.Devices.Aep.SignalStrength",
                "System.Devices.Aep.IsPresent"
            };

            var kind = DeviceInformationKind.AssociationEndpoint;
            listView1.Items.Clear();
            m_listviews = new Dictionary<string, ListViewItem>();
            watcher = DeviceInformation.CreateWatcher(selector, additionalProperties, kind);
            watcher.Added += (s, args) => BeginInvoke(new Action(()=> Watcher_Added(args)));
            watcher.Updated += (s, args) => BeginInvoke(new Action(() => Watcher_Updated(args)));
            watcher.Removed += (s, args) => BeginInvoke(new Action(() => Watcher_Removed(args)));            
            watcher.Start();
            button_start.Enabled = false;
            button_stop.Enabled = true;
        }
        private void Watcher_Added(DeviceInformation args)
        {
            var info = new DeviceInformationDisplay(args);
            var item = listView1.Items.Add(info.Address);
            m_listviews[info.Id] = item;
            item.Tag = info;
            item.SubItems.Add(info.Name);
            item.SubItems.Add(info.Status());
            item.SubItems.Add(info.SignalStrength);
            item.SubItems.Add(info.Id);
        }        
        
        private void Watcher_Updated(DeviceInformationUpdate args)
        {
            ListViewItem item;
            DeviceInformationDisplay info;
            if (m_listviews.ContainsKey(args.Id))
            {
                item = m_listviews[args.Id];
                info = item.Tag as DeviceInformationDisplay;
                info.DeviceInformation.Update(args);
                Debug.WriteLine($"Update {info.Name}");
            }
            else
            {
                Debug.Write($"(New) {args.Id}");
                return;
            }
            item.SubItems[1].Text = info.Name;

            item.SubItems[2].Text = info.Status();
            item.SubItems[3].Text = info.SignalStrength;
            item.SubItems[4].Text = info.Id;
        }
        private void Watcher_Removed(DeviceInformationUpdate args)
        {
            Debug.WriteLine($"Remove {args.Id}");
            if (!m_listviews.ContainsKey(args.Id)) return;
            listView1.Items.Remove(m_listviews[args.Id]);
            m_listviews.Remove(args.Id);
        }

        private void button_stop_Click(object sender, EventArgs e)
        {
            if (watcher != null) watcher.Stop();
            watcher = null;
            
            listView1.Items.Clear();
            m_listviews.Clear();

            button_start.Enabled = true;
            button_Pair.Enabled = button_Unpair.Enabled = button_stop.Enabled = false;
            button_SPP.Enabled = false;

        }

        private async void button_Pair_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;
            var item = listView1.SelectedItems[0];
            var info = item.Tag as DeviceInformationDisplay;
            if (!info.CanPair) return;
            DevicePairingKinds ceremoniesSelected = DevicePairingKinds.ConfirmOnly;
            DevicePairingProtectionLevel protectionLevel = DevicePairingProtectionLevel.Default;
            var customPairing = info.DeviceInformation.Pairing.Custom;
            customPairing.PairingRequested += CustomPairing_PairingRequested;
            button_Pair.Enabled = false;
            var pair_result = await customPairing.PairAsync(ceremoniesSelected, protectionLevel);
            Debug.WriteLine($"PairStatus:{pair_result.Status}");
            customPairing.PairingRequested -= CustomPairing_PairingRequested;
            button_Pair.Enabled = true;
        }
        private void CustomPairing_PairingRequested(DeviceInformationCustomPairing sender, DevicePairingRequestedEventArgs args)
        {
            Debug.WriteLine($"{args.PairingKind}");
            args.Accept();
        }
        private async void button_Unpair_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;
            var item = listView1.SelectedItems[0];
            var info = item.Tag as DeviceInformationDisplay;
            if (!info.IsPaired) return;
            button_Unpair.Enabled = false;
            var result = await info.DeviceInformation.Pairing.UnpairAsync();
            Debug.WriteLine($"UnpairStatus {result}");
            button_Unpair.Enabled = true;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                button_Unpair.Enabled = false;
                button_Pair.Enabled = false;
                button_SPP.Enabled = false;
                return;
            }
            var item = listView1.SelectedItems[0];
            var info = item.Tag as DeviceInformationDisplay;
            button_Pair.Enabled = info.CanPair;
            button_Unpair.Enabled = info.IsPaired;
            button_SPP.Enabled = true;
            foreach(var key in info.Properties.Keys)
            {
                Debug.WriteLine($"Key:{key} Value:{info.GetPropertyForDisplay(key)}");
            }
        }

        private async void button_SPP_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;
            var item = listView1.SelectedItems[0];
            var info = item.Tag as DeviceInformationDisplay;
            button_SPP.Enabled = false;
            try
            {
                DeviceAccessStatus accessStatus = DeviceAccessInformation.CreateFromId(info.Id).CurrentStatus;
                Debug.WriteLine($"{info.Id} {accessStatus}");
                if (accessStatus == DeviceAccessStatus.DeniedByUser)
                {
                    toolStripStatusLabel2.Text = "DeniedByUser";
                    Debug.WriteLine(toolStripStatusLabel2.Text);
                    return;
                }
                using (var bluetoothDevice = await BluetoothDevice.FromIdAsync(info.Id))
                {
                    if (bluetoothDevice == null)
                    {
                        toolStripStatusLabel2.Text = "bluetooth device is null";
                        Debug.WriteLine(toolStripStatusLabel2.Text);
                        return;
                    }
                    toolStripStatusLabel2.Text = $"Device ConnectionStatus {bluetoothDevice.ConnectionStatus}";
                    Debug.WriteLine(toolStripStatusLabel2.Text);
                    if (bluetoothDevice.ConnectionStatus != BluetoothConnectionStatus.Connected)
                    {
                        return;
                    }
                    var uuid = "00000000-0000-1000-8000-00805F9B34FB";
                    var rfcommServices = await bluetoothDevice.GetRfcommServicesForIdAsync(
                        RfcommServiceId.FromUuid(Guid.Parse(uuid)), BluetoothCacheMode.Uncached);
                    if (rfcommServices.Services.Count == 0)
                    {
                        toolStripStatusLabel2.Text = "Can't Get SPP Services";
                        Debug.WriteLine(toolStripStatusLabel2.Text);
                        return;
                    }
                    var chatService = rfcommServices.Services[0];
                    
                    using (var chatSocket = new StreamSocket())
                    {
                        await chatSocket.ConnectAsync(chatService.ConnectionHostName, chatService.ConnectionServiceName);
                        toolStripStatusLabel2.Text = $"Connect to {chatService.ConnectionHostName} {chatService.ConnectionServiceName}";
                        Debug.WriteLine(toolStripStatusLabel2.Text);
                        var dlg = new Form_RemoteCar(chatSocket);
                        dlg.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                toolStripStatusLabel2.Text = ex.Message;
                Debug.WriteLine(ex.Message);
                return;
            }
            finally
            {
                button_SPP.Enabled = true;
            }
        }
    }
}

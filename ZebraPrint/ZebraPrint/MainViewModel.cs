using Android.Bluetooth;
using Android.OS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ZebraPrint
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private const string BT_SERIAL = "00001101-0000-1000-8000-00805f9b34fb";
        private readonly BluetoothAdapter _adapter;

        public MainViewModel()
        {
            _adapter = BluetoothAdapter.DefaultAdapter;

            if (_adapter == null)
                throw new Exception("No Bluetooth adapter found.");

            if (!_adapter.IsEnabled)
                throw new Exception("Bluetooth adapter is not enabled.");
        }
        
        public List<BluetoothDevice> BluetoothPrinters
        {
            get
            {
                return _adapter.BondedDevices.Where(r => r.BluetoothClass.DeviceClass == (DeviceClass)1664).ToList();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private BluetoothDevice _selectedBluetoothPrinter;
        public BluetoothDevice SelectedBluetoothPrinter
        {
            get
            {
                return _selectedBluetoothPrinter;
            }
            set
            {
                if (_selectedBluetoothPrinter != value)
                {
                    _selectedBluetoothPrinter = value;
                    OnPropertyChanged();
                }
            }
        }

        public void PrintBluetooth()
        {
            _adapter.CancelDiscovery();
            
            using (var socket = SelectedBluetoothPrinter.CreateRfcommSocketToServiceRecord(Java.Util.UUID.FromString(BT_SERIAL)))
            using (var writer = new StreamWriter(socket.OutputStream))
            {
                socket.Connect();
                writer.WriteLine("! U1 setvar \"device.languages\" \"zpl\"");
                writer.WriteLine("^XA^FO20,20^A0N,25,25^FDHello World!^FS^XZ");
            }
        }

        private BluetoothSocket BtConnect(BluetoothDevice device)
        {
            BluetoothSocket socket = null;
            ParcelUuid[] uuids = null;
            if (device.FetchUuidsWithSdp())
            {
                uuids = device.GetUuids();
            }
            if ((uuids != null) && (uuids.Length > 0))
            {
                foreach (var uuid in uuids)
                {
                    try
                    {
                        socket = device.CreateRfcommSocketToServiceRecord(uuid.Uuid);
                        socket.Connect();
                        break;
                    }
                    catch (System.Exception ex)
                    {
                        socket = null;
                    }
                }
            }
            return socket;
        }

        private string _tcpHost;
        public string TcpHost
        {
            get
            {
                return _tcpHost;
            }
            set
            {
                if (_tcpHost != value)
                {
                    _tcpHost = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}

using LinkOS.Plugin.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ZebraPrint
{
    public class MainViewModel : INotifyPropertyChanged, IDiscoveryHandler
    {
        public MainViewModel()
        {
            Task.Run(() =>
            {
                LinkOS.Plugin.BluetoothDiscoverer.Current.FindPrinters(Forms.Context, this);
            });
        }

        private List<IDiscoveredPrinterBluetooth> _bluetoothPrinters = new List<IDiscoveredPrinterBluetooth>();

        public List<IDiscoveredPrinterBluetooth> BluetoothPrinters
        {
            get
            {
                return _bluetoothPrinters.ToList();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void FoundPrinter(IDiscoveredPrinter discoveredPrinter)
        {
            if (discoveredPrinter is IDiscoveredPrinterBluetooth bt)
            {
                _bluetoothPrinters.Add(bt);
                OnPropertyChanged(nameof(BluetoothPrinters));
                Debug.WriteLine("Bluetooth device found");
            }
        }

        public void DiscoveryFinished()
        {
            Debug.WriteLine("Bluetooth device discovery finished");
        }

        public void DiscoveryError(string message)
        {
            Debug.WriteLine(message);
        }

        private IDiscoveredPrinterBluetooth _selectedBluetoothPrinter;
        public IDiscoveredPrinterBluetooth SelectedBluetoothPrinter
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

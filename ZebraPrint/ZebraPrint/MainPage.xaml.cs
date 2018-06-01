// Samples found here: http://techdocs.zebra.com/link-os/2-13/android/content/com/zebra/sdk/comm/ConnectionBuilder.html

using LinkOS.Plugin;
using LinkOS.Plugin.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ZebraPrint
{
	public partial class MainPage : ContentPage
	{
        private List<IDiscoveredPrinter> _bluetoothPrinters = new List<IDiscoveredPrinter>();

		public MainPage()
		{
			InitializeComponent();


        }
        
        private MainViewModel ViewModel { get { return BindingContext as MainViewModel; } }

        private void SendBluetooth(object sender, EventArgs e)
        {
            try
            {
                if (ViewModel.SelectedBluetoothPrinter == null)
                {
                    DisplayAlert("Error", "Select bluetooth device first", "Ok");
                    return;
                }

                

                // Instantiate a Bluetooth connection
                var conn = ConnectionBuilder.Current.Build("BT:" + ViewModel.SelectedBluetoothPrinter.Address);
                // Alternative:
                //var conn = ViewModel.SelectedBluetoothPrinter.Connection;

                // Open the connection - physical connection is established here.
                conn.Open();
                
                //Set printer to ZPL mode
                WriteString(conn, "! U1 setvar \"device.languages\" \"zpl\"\r\n");

                WriteString(conn, "^XA^FO20,20^A0N,25,25^FDHello World!^FS^XZ");
                
                // Close the connection to release resources.
                conn.Close();

            }
            catch (Zebra.Sdk.Comm.ConnectionException ex)
            {
                // Handle communications error here.
                Debug.WriteLine(ex.StackTrace);
            }
        }

        private void SendTcp(object sender, EventArgs e)
        {
            // Network connections are not allowed on main UI thread.
            Task.Run(() =>
            {
                try
                {
                    // Instantiate a Bluetooth connection
                    var conn = ConnectionBuilder.Current.Build("TCP:" + ViewModel.TcpHost + ":9100");

                    // Open the connection - physical connection is established here.
                    conn.Open();

                    //Set printer to ZPL mode
                    WriteString(conn, "! U1 setvar \"device.languages\" \"zpl\"\r\n");

                    WriteString(conn, "^XA^FO20,20^A0N,25,25^FDHello World!^FS^XZ");

                    // Close the connection to release resources.
                    conn.Close();
                }
                catch (Zebra.Sdk.Comm.ConnectionException ex)
                {
                    // Handle communications error here.
                    Debug.WriteLine(ex.StackTrace);
                }
            });
        }

        private void WriteString(IConnection connection, string data)
        {
            var bytes = Encoding.UTF8.GetBytes(data);
            connection.Write(bytes);
        }
    }

}

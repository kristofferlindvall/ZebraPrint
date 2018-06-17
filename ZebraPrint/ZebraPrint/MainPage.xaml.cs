// Samples found here: http://techdocs.zebra.com/link-os/2-13/android/content/com/zebra/sdk/comm/ConnectionBuilder.html

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
		public MainPage()
		{
			InitializeComponent();


        }
        
        private MainViewModel ViewModel { get { return BindingContext as MainViewModel; } }

        private void SendBluetooth(object sender, EventArgs e)
        {
            ViewModel.PrintBluetooth();
        }

        private void SendTcp(object sender, EventArgs e)
        {
            ViewModel.PrintNetwork();
        }
    }

}

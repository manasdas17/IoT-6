using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Maker.RemoteWiring;
using Microsoft.Maker.Serial;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ArduinoBTLED
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private RemoteDevice _arduino;

        public MainPage()
        {
            this.InitializeComponent();

            this.Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            var connection = new BluetoothSerial("RNBT-A297");
            _arduino = new RemoteDevice(connection);
            connection.ConnectionEstablished += Connection_ConnectionEstablished;
            connection.begin(0, 0);
        }

        private void Connection_ConnectionEstablished()
        {

        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            _arduino.digitalWrite(3, PinState.LOW);
            _arduino.digitalWrite(10, PinState.LOW);
        }
    }
}

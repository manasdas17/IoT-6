using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MqttEmotionViewer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private MqttClient _mqtt;
        private SolidColorBrush _redBrush = new SolidColorBrush(Colors.Red);
        private SolidColorBrush _greenBrush = new SolidColorBrush(Colors.Lime);

        public MainPage()
        {
            this.InitializeComponent();

            disconnected();

            _mqtt = new MqttClient("m11.cloudmqtt.com", 12360, false, MqttSslProtocols.None);
            _mqtt.ConnectionClosed += (sender, args) => { disconnected(); };
            _mqtt.MqttMsgSubscribed += (sender, args) =>
            {
                connected();
            };
            _mqtt.MqttMsgPublishReceived += (sender, args) =>
                            {
                var msg = Encoding.ASCII.GetString(args.Message);

                switch (msg)
                {
                    case "happy":
                        {
                        }
                        break;
                    case "sad":
                    {
                    }
                        break;
                    case "indifferent":
                        {
                        }
                        break;
                }
            };

            Task.Run(() =>
            {
                try
                {
                    _mqtt.Connect("1", "mike", "cloudmqtt");
                    _mqtt.Subscribe(new[] {"mqttdotnet/pubtest/#"}, new[] {MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE});
                }
                catch (Exception ex)
                {
                    
                }
            });

        }

        private void connected()
        {
            Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                tbConnectedOrNot.Text = "Connected";
                tbConnectedOrNot.Foreground = _greenBrush;
            });
        }

        private void disconnected()
        {
            Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                tbConnectedOrNot.Text = "Disconnected";
                tbConnectedOrNot.Foreground = _redBrush;
            });
        }
    }
}

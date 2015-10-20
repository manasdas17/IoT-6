using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ST.IoT.Demos.Utils;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Core;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace EmoticonMqttWinIoT
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Adafrut8x8LEDBackpack _backpack;
        private MqttClient _mqtt;
        private AnimationFrame[] _frames;
        private BackpackAnimator _animator;
        private OneShotPeriodicTimer _clear = new OneShotPeriodicTimer();
        private SolidColorBrush _redBrush = new SolidColorBrush(Colors.Red);
        private SolidColorBrush _greenBrush = new SolidColorBrush(Colors.Lime);

        public MainPage()
        {
            this.InitializeComponent();

            Loaded += MainPage_Loaded;
        }

        private async void MainPage_Loaded(object s, RoutedEventArgs e)
        {
            disconnected();

            _mqtt = new MqttClient("m11.cloudmqtt.com", 12360, false, MqttSslProtocols.None);
            _mqtt.ConnectionClosed += (sender, args) =>
            {
                disconnected();
            };
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
                        _animator.drawFrame(0);
                    }
                    break;
                    case "sad":
                    {
                        _animator.drawFrame(1);
                    }
                    break;
                    case "indifferent":
                    {
                        _animator.drawFrame(2);
                    }
                    break;
                }

                _clear.schedule(() => _backpack.clear(), 5000);
            };

            Task.Run(() =>
            {

                _mqtt.Connect("1", "mike", "cloudmqtt");
                _mqtt.Subscribe(new[] {"mqttdotnet/pubtest/#"}, new[] {MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE});
            });

            _backpack = new Adafrut8x8LEDBackpack();
            await _backpack.initializeAsync();

            _animator = new BackpackAnimator(_backpack, EmoticonsFrameGenerator.getAnimationFrames());
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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Gpio;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Blinky.Complete
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private const int LED_PIN = 5;
        private GpioPin _pin;
        private SolidColorBrush _yellowBrush = new SolidColorBrush(Windows.UI.Colors.Yellow);
        private SolidColorBrush _grayBrush = new SolidColorBrush(Windows.UI.Colors.LightGray);

        public MainPage()
        {
            this.InitializeComponent();

            InitGPIO();
            StartBlinking();
        }

        private void InitGPIO()
        {
            var gpio = GpioController.GetDefault();
            _pin = gpio.OpenPin(LED_PIN);
            _pin.Write(GpioPinValue.Low);
            _pin.SetDriveMode(GpioPinDriveMode.Output);
        }

        private void StartBlinking()
        {
            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.5);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, object e)
        {
            var pinValue = _pin.Read();
            _pin.Write(pinValue == GpioPinValue.High ? GpioPinValue.Low : GpioPinValue.High);
            LED.Fill = pinValue == GpioPinValue.High ? _yellowBrush : _grayBrush;
        }
    }
}

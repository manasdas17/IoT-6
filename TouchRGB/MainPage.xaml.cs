using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Gpio;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.SpeechRecognition;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TouchRGB
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private GpioPin _greenPin;
        private GpioPin _redPin;
        private GpioPin _bluePin;

        public MainPage()
        {
            this.InitializeComponent();

            red.Opacity = 0.1;
            green.Opacity = 0.1;
            blue.Opacity = 0.1;

            InitGPIO();
        }

        private void InitGPIO()
        {
            var gpio = GpioController.GetDefault();
            _greenPin = gpio.OpenPin(18);
            _bluePin = gpio.OpenPin(23);
            _redPin = gpio.OpenPin(24);

            _greenPin.Write(GpioPinValue.Low);
            _greenPin.SetDriveMode(GpioPinDriveMode.Output);
            _redPin.Write(GpioPinValue.Low);
            _redPin.SetDriveMode(GpioPinDriveMode.Output);
            _bluePin.Write(GpioPinValue.Low);
            _bluePin.SetDriveMode(GpioPinDriveMode.Output);
        }


        private void green_OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            green.Opacity = green.Opacity < 1 ? 1.0 : 0.1;

            setcolors();
        }

        private void blue_OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            blue.Opacity = blue.Opacity < 1 ? 1.0 : 0.1;
            setcolors();
        }

        private void red_OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            red.Opacity = red.Opacity < 1 ? 1.0 : 0.1;
            setcolors();
        }

        private void setcolors()
        {
            var useRed = red.Opacity > 0.9;
            var useGreen = green.Opacity > 0.9;
            var useBlue = blue.Opacity > 0.9;

            fill.Fill =
                new SolidColorBrush(Color.FromArgb(255, useRed ? (byte) 255 : (byte) 0, useGreen ? (byte) 255 : (byte) 0,
                    useBlue ? (byte) 255 : (byte) 0));

            _redPin.Write(useRed ? GpioPinValue.High : GpioPinValue.Low);
            _greenPin.Write(useGreen ? GpioPinValue.High : GpioPinValue.Low);
            _bluePin.Write(useBlue ? GpioPinValue.High : GpioPinValue.Low);
        }
    }
}

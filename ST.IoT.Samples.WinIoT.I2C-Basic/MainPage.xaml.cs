using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using Windows.Devices.I2c;
using Windows.UI.Text.Core;
using ST.IoT.Samples.WinIoT.I2C.Utils;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ST.IoT.Samples.WinIoT.I2C_Basic
{

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            _mqtt = new MqttClient("m11.cloudmqtt.com", 12360, false, MqttSslProtocols.None);
            _mqtt.ConnectionClosed += (sender, args) => { };
            _mqtt.MqttMsgSubscribed += (sender, args) => { };
            _mqtt.MqttMsgPublishReceived += (sender, args) =>
            {

            };
            _mqtt.Connect("1", "mike", "cloudmqtt");
            _mqtt.Subscribe(new[] { "mqttdotnet/pubtest/#" }, new[] { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE });

            btnStart.IsEnabled = true;
            btnStop.IsEnabled = false;

            initGPIO();
            initI2C();
        }

        private void initGPIO()
        {
            
        }

        private const byte __HT16K33_REGISTER_DISPLAY_SETUP = 0x80;
        private const byte __HT16K33_REGISTER_SYSTEM_SETUP = 0x20;
        private const byte __HT16K33_REGISTER_DIMMING = 0xE0;

        private const byte PORT_EXPANDER_IODIR_REGISTER_ADDRESS = 0x00;
        private const byte PORT_EXPANDER_GPIO_REGISTER_ADDRESS = 0x09;
        private const byte PORT_EXPANDER_OLAT_REGISTER_ADDRESS = 0x09;

        private const byte __HT16K33_BLINKRATE_OFF = 0x00;
        private const byte __HT16K33_BLINKRATE_2HZ = 0x01;
        private const byte __HT16K33_BLINKRATE_1HZ = 0x02;
        private const byte __HT16K33_BLINKRATE_HALFHZ = 0x03;

        private byte[] _buffer = {0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0};

        private I2cDevice _device;

        public class Frame
        {
            public byte[] Values { get; set; }
            public int Duration { get; set; }
        }

        public class AnimationFrames
        {
            public Frame[] Frames { get; set; }
        }

        private AnimationFrames _spaceInvaderFrames = new AnimationFrames()
        {
            Frames = new[]
            {
                new Frame() { Values = new[] {
                      BinaryValues.b00011000,
                      BinaryValues.b00111100,
                      BinaryValues.b01111110,
                      BinaryValues.b11011011,
                      BinaryValues.b11111111,
                      BinaryValues.b00100100,
                      BinaryValues.b01011010,
                      BinaryValues.b10100101
                },
                Duration = 250},
                new Frame() { Values = new[] {
                      BinaryValues.b00011000,
                      BinaryValues.b00111100,
                      BinaryValues.b01111110,
                      BinaryValues.b11011011,
                      BinaryValues.b11111111,
                      BinaryValues.b00100100,
                      BinaryValues.b01011010,
                      BinaryValues.b01000010,
                },
                Duration = 250},
                new Frame() { Values = new[] {
                      BinaryValues.b00011000,
                      BinaryValues.b00111100,
                      BinaryValues.b01111110,
                      BinaryValues.b11011011,
                      BinaryValues.b11111111,
                      BinaryValues.b00100100,
                      BinaryValues.b01011010,
                      BinaryValues.b10100101
                },
                Duration = 250},
                new Frame() { Values = new[] {
                      BinaryValues.b00011000,
                      BinaryValues.b00111100,
                      BinaryValues.b01111110,
                      BinaryValues.b11011011,
                      BinaryValues.b11111111,
                      BinaryValues.b00100100,
                      BinaryValues.b01011010,
                      BinaryValues.b01000010,
                },
                Duration = 250},

                // 2nd alien
                new Frame() { Values = new[] {
                    BinaryValues.b00000000,
                    BinaryValues.b00111100,
                    BinaryValues.b01111110,
                    BinaryValues.b11011011,
                    BinaryValues.b11011011,
                    BinaryValues.b01111110,
                    BinaryValues.b00100100,
                    BinaryValues.b11000011,
                },
                Duration = 250},
                new Frame() { Values = new[] {
                    BinaryValues.b00111100,
                    BinaryValues.b01111110,
                    BinaryValues.b11011011,
                    BinaryValues.b11011011,
                    BinaryValues.b01111110,
                    BinaryValues.b00100100,
                    BinaryValues.b00100100,
                    BinaryValues.b00100100,
                },
                Duration = 250},
                new Frame() { Values = new[] {
                    BinaryValues.b00000000,
                    BinaryValues.b00111100,
                    BinaryValues.b01111110,
                    BinaryValues.b11011011,
                    BinaryValues.b11011011,
                    BinaryValues.b01111110,
                    BinaryValues.b00100100,
                    BinaryValues.b11000011,
                },
                Duration = 250},
                new Frame() { Values = new[] {
                    BinaryValues.b00111100,
                    BinaryValues.b01111110,
                    BinaryValues.b11011011,
                    BinaryValues.b11011011,
                    BinaryValues.b01111110,
                    BinaryValues.b00100100,
                    BinaryValues.b00100100,
                    BinaryValues.b00100100,
                },
                Duration = 250},

                // 3rd alien
                new Frame() { Values = new[] {
                      BinaryValues.b00100100, 
                      BinaryValues.b00100100,
                      BinaryValues.b01111110,
                      BinaryValues.b11011011,
                      BinaryValues.b11111111,
                      BinaryValues.b11111111,
                      BinaryValues.b10100101,
                      BinaryValues.b00100100,
                },
                Duration = 250},
                new Frame() { Values = new[] {
                      BinaryValues.b00100100, 
                      BinaryValues.b10100101,
                      BinaryValues.b11111111,
                      BinaryValues.b11011011,
                      BinaryValues.b11111111,
                      BinaryValues.b01111110,
                      BinaryValues.b00100100,
                      BinaryValues.b01000010,
                },
                Duration = 250},

                new Frame() { Values = new[] {
                      BinaryValues.b00100100,
                      BinaryValues.b00100100,
                      BinaryValues.b01111110,
                      BinaryValues.b11011011,
                      BinaryValues.b11111111,
                      BinaryValues.b11111111,
                      BinaryValues.b10100101,
                      BinaryValues.b00100100,
                },
                Duration = 250},
                new Frame() { Values = new[] {
                      BinaryValues.b00100100,
                      BinaryValues.b10100101,
                      BinaryValues.b11111111,
                      BinaryValues.b11011011,
                      BinaryValues.b11111111,
                      BinaryValues.b01111110,
                      BinaryValues.b00100100,
                      BinaryValues.b01000010,
                },
                    Duration = 250},


                // 4th alien
                new Frame() { Values = new[] {
  BinaryValues.b00111100,
  BinaryValues.b01111110,
  BinaryValues.b00110011,
  BinaryValues.b01111110,
  BinaryValues.b00111100,
  BinaryValues.b00000000,
  BinaryValues.b00001000,
  BinaryValues.b00000000,
                },
                    Duration = 125},
                new Frame() { Values = new[] {
  BinaryValues.b00111100,
  BinaryValues.b01111110,
  BinaryValues.b10011001,
  BinaryValues.b01111110,
  BinaryValues.b00111100,
  BinaryValues.b00000000,
  BinaryValues.b00001000,
  BinaryValues.b00001000,
                },
                    Duration = 125},
                new Frame() { Values = new[] {
  BinaryValues.b00111100,
  BinaryValues.b01111110,
  BinaryValues.b11001100,
  BinaryValues.b01111110,
  BinaryValues.b00111100,
  BinaryValues.b00000000,
  BinaryValues.b00000000,
  BinaryValues.b00001000,
                },
                    Duration = 125},
                new Frame() { Values = new[] {
  BinaryValues.b00111100,
  BinaryValues.b01111110,
  BinaryValues.b01100110,
  BinaryValues.b01111110,
  BinaryValues.b00111100,
  BinaryValues.b00000000,
  BinaryValues.b00000000,
  BinaryValues.b00000000,
                },
                    Duration = 125},                new Frame() { Values = new[] {
  BinaryValues.b00111100,
  BinaryValues.b01111110,
  BinaryValues.b00110011,
  BinaryValues.b01111110,
  BinaryValues.b00111100,
  BinaryValues.b00000000,
  BinaryValues.b00001000,
  BinaryValues.b00000000,
                },
                    Duration = 125},
                new Frame() { Values = new[] {
  BinaryValues.b00111100,
  BinaryValues.b01111110,
  BinaryValues.b10011001,
  BinaryValues.b01111110,
  BinaryValues.b00111100,
  BinaryValues.b00000000,
  BinaryValues.b00001000,
  BinaryValues.b00001000,
                },
                    Duration = 125},
                new Frame() { Values = new[] {
  BinaryValues.b00111100,
  BinaryValues.b01111110,
  BinaryValues.b11001100,
  BinaryValues.b01111110,
  BinaryValues.b00111100,
  BinaryValues.b00000000,
  BinaryValues.b00000000,
  BinaryValues.b00001000,
                },
                    Duration = 125},
                new Frame() { Values = new[] {
  BinaryValues.b00111100,
  BinaryValues.b01111110,
  BinaryValues.b01100110,
  BinaryValues.b01111110,
  BinaryValues.b00111100,
  BinaryValues.b00000000,
  BinaryValues.b00000000,
  BinaryValues.b00000000,
                },
                    Duration = 125},

  // faster repeat of 1-4
                  new Frame() { Values = new[] {
                      BinaryValues.b00011000,
                      BinaryValues.b00111100,
                      BinaryValues.b01111110,
                      BinaryValues.b11011011,
                      BinaryValues.b11111111,
                      BinaryValues.b00100100,
                      BinaryValues.b01011010,
                      BinaryValues.b10100101
                },
                Duration = 125},
                new Frame() { Values = new[] {
                      BinaryValues.b00011000,
                      BinaryValues.b00111100,
                      BinaryValues.b01111110,
                      BinaryValues.b11011011,
                      BinaryValues.b11111111,
                      BinaryValues.b00100100,
                      BinaryValues.b01011010,
                      BinaryValues.b01000010,
                },
                Duration = 125},
                new Frame() { Values = new[] {
                      BinaryValues.b00011000,
                      BinaryValues.b00111100,
                      BinaryValues.b01111110,
                      BinaryValues.b11011011,
                      BinaryValues.b11111111,
                      BinaryValues.b00100100,
                      BinaryValues.b01011010,
                      BinaryValues.b10100101
                },
                Duration = 125},
                new Frame() { Values = new[] {
                      BinaryValues.b00011000,
                      BinaryValues.b00111100,
                      BinaryValues.b01111110,
                      BinaryValues.b11011011,
                      BinaryValues.b11111111,
                      BinaryValues.b00100100,
                      BinaryValues.b01011010,
                      BinaryValues.b01000010,
                },
                Duration = 125},

            }
        };

        private async void initI2C()
        {
            string aqs = I2cDevice.GetDeviceSelector("I2C1");

            // Find the I2C bus controller with our selector string
            var dis = await DeviceInformation.FindAllAsync(aqs);
            if (dis.Count == 0)
                return; // bus not found

            // 0x40 is the I2C device address
            var settings = new I2cConnectionSettings(0x70);

            // this device is actually a port expander

            // Create an I2cDevice with our selected bus controller and I2C settings
            _device = await I2cDevice.FromIdAsync(dis[0].Id, settings);
            /*
                var i2c_rb = new byte[1];

                device.WriteRead(new byte[] { PORT_EXPANDER_IODIR_REGISTER_ADDRESS }, i2c_rb);
                var io_dir_reg = i2c_rb[0];

                device.WriteRead(new byte[] { PORT_EXPANDER_GPIO_REGISTER_ADDRESS }, i2c_rb);
                var gpio_reg = i2c_rb[0];

                device.WriteRead(new byte[] { PORT_EXPANDER_OLAT_REGISTER_ADDRESS }, i2c_rb);
                var olat_reg = i2c_rb[0];
                i2c_rb = null;
                */


                write8(_device, __HT16K33_REGISTER_SYSTEM_SETUP | 0x01, 0x00);
                setBlinkRate(_device, __HT16K33_BLINKRATE_OFF);
                setBrightness(_device, 15);
                clear(_device);

                //PeriodicTaskFactory.Start(draw, 1000);

            //draw();
        }


        private bool _run = false;
        private int _frame = 0;
        private void drawSpaceInvader()
        {
            if (!_run)
            {
                clear(_device);
                return;
            }
            //Array.Copy(_spaceInvaderFrames.Frames[0].Values, _buffer, 8);
            for (var i = 0; i < 8; i++)
            {
                var b = _spaceInvaderFrames.Frames[_frame].Values[i];
                _buffer[i] = (byte)((b >> 1) | (b << 7));
            }
            writeDisplay(_device);
            Task.Delay(_spaceInvaderFrames.Frames[_frame].Duration).ContinueWith(_ => drawSpaceInvader());
;            _frame = (_frame + 1) % _spaceInvaderFrames.Frames.Length;
        }

        private int count = 0;
        private MqttClient _mqtt;

        private void draw()
        {
            //clear(_device, false);

            var x = (count < 8) ? count : 15 - count - 1;
            for (var y = 0; y < 8; y++) setPixel(_device, x, y);

            writeDisplay(_device);

            Task.Delay(_spaceInvaderFrames.Frames[_frame].Duration).ContinueWith(_ =>
            {
                count++;
                count = count % 15;
                if (count == 0) count++;
                draw();
            });
        }

        private void clear(I2cDevice device, bool update=true)
        {
            Array.Clear(_buffer, 0, _buffer.Length);
            
            if (update) writeDisplay(device);
        }

        private void writeDisplay(I2cDevice device)
        {
            var bytes = new List<byte>();
            foreach (var b in _buffer)
            {
                bytes.Add((byte)(b & (byte)0xff));
                bytes.Add((byte)((b>>8) & (byte)0xff));
            }
            writeList(device, 0x0, bytes);
        }

        private void writeList(I2cDevice device, byte reg, List<byte> bytes)
        {
            var l = new List<byte>(bytes);
            l.Insert(0, reg);
            device.Write(l.ToArray());
        }

        private void setBrightness(I2cDevice device, byte brightness)
        {
            if (brightness > 15) brightness = 15;
            write8(device, (byte)(__HT16K33_REGISTER_DIMMING | brightness), 0x00);
        }

        private void setBlinkRate(I2cDevice device, byte blinkRate)
        {
            if (blinkRate > __HT16K33_BLINKRATE_HALFHZ) blinkRate = __HT16K33_BLINKRATE_OFF;
            var reg = (byte) (__HT16K33_REGISTER_DISPLAY_SETUP | 0x01 | (blinkRate << 1));
            write8(device, reg, 0x00);
        }

        private void write8(I2cDevice device, byte reg, byte value)
        {
            device.Write(new []{ reg, value});
        }

        private void setPixel(I2cDevice device, int x, int y, bool color = true, bool update = true)
        {
            if (x >= 8) return;
            if (y >= 8) return;
            x += 7;
            x %= 8;
            if (color)
            {
                setBufferRow(device, y, (byte)(_buffer[y] | (byte)(1 << x)), update);
            }
            else
            {
                setBufferRow(device, y, (byte)(_buffer[y] | (byte)(~(1 << x))));
            }
        }

        private void setBufferRow(I2cDevice device, int row, byte value, bool update = true)
        {
            if (row > 7) return;
            _buffer[row] = value;
            if (update) writeDisplay(device);
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            btnStart.IsEnabled = false;
            btnStop.IsEnabled = true;
            _run = true;
            drawSpaceInvader();
        }

        private void btnStop_OnClick(object sender, RoutedEventArgs e)
        {
            _run = false;
            btnStart.IsEnabled = true;
            btnStop.IsEnabled = false;
        }
    }
}

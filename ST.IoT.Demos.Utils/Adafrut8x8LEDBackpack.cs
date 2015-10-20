using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.I2c;

namespace ST.IoT.Demos.Utils
{
    public class Adafrut8x8LEDBackpack
    {
        private byte[] _buffer = { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 };

        private const byte __HT16K33_REGISTER_DISPLAY_SETUP = 0x80;
        private const byte __HT16K33_REGISTER_SYSTEM_SETUP = 0x20;
        private const byte __HT16K33_REGISTER_DIMMING = 0xE0;

        private const byte __HT16K33_BLINKRATE_OFF = 0x00;
        private const byte __HT16K33_BLINKRATE_2HZ = 0x01;
        private const byte __HT16K33_BLINKRATE_1HZ = 0x02;
        private const byte __HT16K33_BLINKRATE_HALFHZ = 0x03;

        private I2cDevice _device;

        public async Task initializeAsync()
        {
            _device = await new I2CManager().initializeDevice(0x70);
            write8(__HT16K33_REGISTER_SYSTEM_SETUP | 0x01, 0x00);
            setBlinkRate(__HT16K33_BLINKRATE_OFF);
            setBrightness(15);
            clear();

        }

        public void drawFrame(byte[] buffer)
        {
            Array.Copy(buffer, _buffer, buffer.Length);
            writeDisplay();
        }

        public void clear(bool update = true)
        {
            Array.Clear(_buffer, 0, _buffer.Length);

            if (update) writeDisplay();
        }

        public void writeDisplay()
        {
            var bytes = new List<byte>();
            foreach (var b in _buffer)
            {
                bytes.Add((byte)(b & (byte)0xff));
                bytes.Add((byte)((b >> 8) & (byte)0xff));
            }
            writeList(0x0, bytes);
        }

        public void writeList(byte reg, List<byte> bytes)
        {
            var l = new List<byte>(bytes);
            l.Insert(0, reg);
            _device.Write(l.ToArray());
        }

        public void setBrightness(byte brightness)
        {
            if (brightness > 15) brightness = 15;
            write8((byte)(__HT16K33_REGISTER_DIMMING | brightness), 0x00);
        }

        public void setBlinkRate(byte blinkRate)
        {
            if (blinkRate > __HT16K33_BLINKRATE_HALFHZ) blinkRate = __HT16K33_BLINKRATE_OFF;
            var reg = (byte)(__HT16K33_REGISTER_DISPLAY_SETUP | 0x01 | (blinkRate << 1));
            write8(reg, 0x00);
        }

        public void write8(byte reg, byte value)
        {
            _device.Write(new[] { reg, value });
        }

        public void setPixel(int x, int y, bool color = true, bool update = true)
        {
            if (x >= 8) return;
            if (y >= 8) return;
            x += 7;
            x %= 8;
            if (color)
            {
                setBufferRow(y, (byte)(_buffer[y] | (byte)(1 << x)), update);
            }
            else
            {
                setBufferRow(y, (byte) (_buffer[y] & (byte) (~(1 << x))), update);
            }
        }

        public void setBufferRow(int row, byte value, bool update = true)
        {
            if (row > 7) return;
            _buffer[row] = value;
            if (update) writeDisplay();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.I2c;

namespace ST.IoT.Demos.Utils
{
    public class I2CManager
    {
        private object _device;

        public async Task<I2cDevice> initializeDevice(int address)
        {
            var aqs = I2cDevice.GetDeviceSelector("I2C1");

            // Find the I2C bus controller with our selector string
            var dis = await DeviceInformation.FindAllAsync(aqs);
            if (dis.Count == 0)
                return null; // bus not found

            // 0x40 is the I2C device address
            var settings = new I2cConnectionSettings(0x70);

            // this device is actually a port expander

            // Create an I2cDevice with our selected bus controller and I2C settings
            return await I2cDevice.FromIdAsync(dis[0].Id, settings);
        }
    }
}

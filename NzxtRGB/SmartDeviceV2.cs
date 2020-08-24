using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace NzxtRGB
{
    public class SmartDeviceV2 : NzxtDevice
    {
        protected override int VendorId => 0x1E71;
        protected override int ProductId => 0x2006;

        public NzxtLedDevice Channel1 { get; private set; }
        public NzxtLedDevice Channel2 { get; private set; }
        public bool Ready { get; private set; }


        /// <summary>
        /// Opens the device asynchronously. If there's a problem opening the device, this returns null
        /// </summary>
        public static async Task<SmartDeviceV2> OpenDeviceAsync(NzxtDeviceConfig openConfig = null)
        {
            SmartDeviceV2 device = new SmartDeviceV2(openConfig);
            bool success = await device.OpenHidDevice().ConfigureAwait(false);
            await device.QueryDevices().ConfigureAwait(false);
            if (success)
                return device;
            else
                return null;
        }

        private SmartDeviceV2() : base() { }

        private SmartDeviceV2(NzxtDeviceConfig config) : base(config) { }

        /// <summary>
        /// Query the devices connected. Not ready until this finishes.
        /// </summary>
        private async Task QueryDevices()
        {
            await Task.Run(async () =>
            {
                DeviceStream.Write(new byte[] { 0x20, 0x03 });
                while (true)
                {
                    byte[] readBuf = DeviceStream.Read();

                    if (readBuf[0] == 0x21 && readBuf[1] == 0x03)
                    {
                        // We received the device query reply
                        byte[] device1 = new byte[6];
                        byte[] device2 = new byte[6];
                        Array.Copy(readBuf, 15, device1, 0, 6);
                        Array.Copy(readBuf, 21, device2, 0, 6);
                        string deviceStr1 = Util.ByteArrayToString(device1);
                        string deviceStr2 = Util.ByteArrayToString(device2);

                        Channel1 = NzxtLedDevice.FromDeviceIdentifier(deviceStr1);
                        Channel2 = NzxtLedDevice.FromDeviceIdentifier(deviceStr2);

                        break;
                    }
                    await Task.Delay(10).ConfigureAwait(false);
                }
                Ready = true;

            });
        }

        /// <summary>
        /// Sends an array of colors
        /// </summary>
        public void SendRGB(byte channel, Color[] colors)
        {
            List<byte> bytes = new List<byte>();
            foreach(Color c in colors)
            {
                bytes.Add(c.R);
                bytes.Add(c.G);
                bytes.Add(c.B);
            }
            SendRGB(channel, bytes.ToArray());
        }

        /// <summary>
        /// Send color array in RGB format
        /// </summary>
        public void SendRGB(byte channel, byte[] colors)
        {
            if (channel != 1 && channel != 2)
                throw new ArgumentException("Channel must be 1 or 2");

            if (colors.Length % 3 != 0)
                throw new ArgumentException("Color array length must be multiple of 3");

            byte[] buffer = null;
            using (MemoryStream strm = new MemoryStream(4 + colors.Length * 3))
            {
                strm.Write(new byte[] { 0x22, 0x10, channel, 0x00 }, 0, 4);
                for (int i = 0; i < colors.Length/3; i += 1)
                {
                    strm.Write(new byte[] { colors[i*3+1], colors[i * 3], colors[i * 3 + 2] }, 0, 3);
                }
                buffer = strm.ToArray();
            }
            DeviceStream.Write(buffer);
            DeviceStream.Write(new byte[] { 0x22, 0x11, channel });
            //0x22,0xA0,0x02,0x00,0x01,0x00,0x00,0x10,0x00,0x00,0x80,0x00,0x32,0x00,0x00,0x01
            DeviceStream.Write(new byte[] { 0x22, 0xA0, channel, 0x00,0x01,0x00,0x00,0x10,0x00,0x00,0x80,0x00,0x32,0x00,0x00,0x01 });

        }

    }
}

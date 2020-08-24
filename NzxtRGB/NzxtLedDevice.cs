using NzxtRGB.LedDevices;
using System;
using System.Collections.Generic;
using System.Text;

namespace NzxtRGB
{
    public class NzxtLedDevice
    {
        public virtual string Name { get; }
        public virtual int LedCount { get; }

        internal static NzxtLedDevice FromDeviceIdentifier(string deviceId)
        {
            switch (deviceId)
            {
                case "040000000000":
                    return new StripV2();
                case "0C0C00000000":
                    return new Aer2();
                default:
                    return null; // No device connected or unknown
            }
        }

        public override string ToString()
        {
            return $"Name={Name} LedCount={LedCount}";
        }

    }
}

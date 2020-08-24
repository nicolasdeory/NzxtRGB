using System;
using System.Collections.Generic;
using System.Text;

namespace NzxtRGB.LedDevices
{
    class Aer2 : NzxtLedDevice
    {
        public override string Name => "Aer 2";
        public override int LedCount => 16; // 8 LED x 2 fans
    }
}

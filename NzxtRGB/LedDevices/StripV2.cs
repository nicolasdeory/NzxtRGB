using System;
using System.Collections.Generic;
using System.Text;

namespace NzxtRGB.LedDevices
{
    class StripV2 : NzxtLedDevice
    {
        public override string Name => "Strip V2";
        public override int LedCount => 10; // 8 LED x 2 fans
    }
}

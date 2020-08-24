using System;
using System.Collections.Generic;
using System.Text;

namespace NzxtRGB
{
    public class NzxtDeviceConfig
    {
        /// <summary>
        /// Restarts NZXT Cam silently if the device can't be accessed. NZXT Cam opens devices in exclusive mode by default,
        /// so if it's running before this one, you won't be able to open the devices.
        /// </summary>
        public bool RestartNzxtCamOnFail { get; set; } = false;

        /// <summary>
        /// Restarts NZXT Cam when the device is closed. No effect unless <seealso cref="RestartNzxtCamOnFail"/> is set to true. 
        /// If you're using multiple devices, you should only enable this on just one, so we don't trigger multiple restarts.
        /// </summary>
        public bool RestartNzxtOnClose { get; set; } = false;
    }
}

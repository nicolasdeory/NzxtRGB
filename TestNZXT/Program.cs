using HidSharp;
using NzxtRGB;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestNZXT
{
    /** 28 03 02 10 00 00 00 00 01 00 00 51 7A            (..........Qz
     * This is fixed mode
     */

    /**  28 03 02 10 0C 00 00 00 01                        (........
     * This is super rainbow mode
     */

    class Program
    {
        static void Main(string[] args)
        {

             // If you want to get rid of the NZXT CAM warning, you need to restart it again when exiting*/

            NzxtDeviceConfig config = new NzxtDeviceConfig();
            config.RestartNzxtCamOnFail = true;
            config.RestartNzxtOnClose = true;
            SmartDeviceV2 device = SmartDeviceV2.OpenDeviceAsync(config).Result;
            Console.WriteLine("channel 1 " + device.Channel1);
            Console.WriteLine("channel 2 " + device.Channel2);

            List<Color> ch1Colors = new List<Color>();
            for(int i = 0; i < device.Channel1.LedCount; i++)
            {
                ch1Colors.Add(Color.FromArgb(255, 255, 0));
            }

            List<Color> ch2Colors = new List<Color>();
            for (int i = 0; i < device.Channel2.LedCount; i++)
            {
                ch2Colors.Add(Color.FromArgb(255, 0, 255));
            }

            Task.Run(async () =>
            {
                while (true)
                {
                    device.SendRGB(1, ch1Colors.ToArray());
                    device.SendRGB(2, ch2Colors.ToArray());
                    await Task.Delay(500);
                }
            });


            Console.Read();
            device.Dispose();

        }
    }
}

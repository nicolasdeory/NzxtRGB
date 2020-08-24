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


    /**
     *  22 10 02 00 FF FF FF FF FF FF FF FF FF FF FF FF   "...ÿÿÿÿÿÿÿÿÿÿÿÿ
FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF   ÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿ
FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF   ÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿ
FF FF FF FF                                       ÿÿÿÿ

        Fixed white packet 1 (16 x 3 GRB values)
        */

    /**
      22 11 02                                          "..
    fixed packet 2
     */

    /**
     *  22 A0 02 00 01 00 00 10 00 00 80 00 32 00 00 01   " ........€.2...
        fixed packet 3
     */

    /** 
     * 60 02 01 E8 03 01 E8 03 00 00
     * or
     * 60 03               
     * or
     * 20 03                                               .`.
     * Device query request (replies with device ids)
     */

    class Program
    {
        /// <summary>
        /// Blocks for 0.5s 
        /// </summary>
        static void RestartNZXT()
        {
            Process[] processes = Process.GetProcessesByName("NZXT CAM");
            Process nzxtCam = processes[0];
            string path = nzxtCam.MainModule.FileName;
            nzxtCam.Kill();
            Process.Start(path, "--startup");
        }

        static void Main(string[] args)
        {
            /* byte[] fixedbuf = new byte[] { 0x28, 0x03, 0x02, 0x10, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x51, 0x7A };
             byte[] rainbowbuf = new byte[] { 0x28, 0x03, 0x02, 0x10, 0x0C, 0x00, 0x00, 0x00, 0x01 };


             byte[] querybuf = new byte[] { 0x20, 0x03 };
             byte[] querybuf2 = new byte[] { 0x60, 0x02, 0x01, 0xE8, 0x03, 0x01, 0xE8, 0x03 };


             byte[] rgbbuf = new byte[] {
             0x22, 0x10, 0x02, 0x00,
             0xFF, 0x00, 0x00,
             0x00, 0xFF, 0x00,
             0x00, 0x00, 0xFF,
             0x00, 0xFF, 0x00,
             0x00, 0xFF, 0x00,
             0x00, 0xFF, 0x00,
             0xFF, 0x00, 0x00,
             0xFF, 0x00, 0x00,
             0xFF, 0x00, 0x00,
             0xFF, 0x00, 0x00,
             0xFF, 0x00, 0x00,
             0xFF, 0x00, 0x00,
             0xFF, 0x00, 0x00,
             0xFF, 0x00, 0x00,
             0xFF, 0x00, 0x00,
             0xFF, 0x00, 0x00,
         };

             byte[] rgbbuf2 = new byte[]
             {
             0x22, 0x11, 0x02
             };

             //22 A0 02 00 01 00 00 10 00 00 80 00 32 00 00 01
             byte[] rgbbuf3 = new byte[]
             {
                 0x22,0xA0,0x02,0x00,0x01,0x00,0x00,0x10,0x00,0x00,0x80,0x00,0x32,0x00,0x00,0x01
             };



             byte[] buftosend = rgbbuf;

             Console.WriteLine("Hello World!");
             HidDevice d = DeviceList.Local.GetHidDeviceOrNull(0x1E71, 0x2006);
             Console.WriteLine(d.GetManufacturer());
             HidStream s;

             if (!d.TryOpen(out s))
             {
                 Console.WriteLine("Couldnt open device, restarting NZXT CAM");
                 RestartNZXT();
                 Process[] processes = Process.GetProcessesByName("NZXT CAM");
                 Process nzxtCam = processes[0];
                 string path = nzxtCam.MainModule.FileName;
                 nzxtCam.Kill();
                 Thread.Sleep(500);
                 s = d.Open();
                 Process.Start(path, "--startup");


             }

             if (s.CanWrite)
             {
                 Task.Run(async () =>
                 {
                     s.Write(querybuf);
                   //  s.Write(querybuf2);

                     while (true)
                     {
                         byte[] readBuf = s.Read();

                         if (readBuf[0] == 0x21 && readBuf[1] == 0x03)
                         {
                             Console.WriteLine("received query reply");
                             string str = ByteArrayToString(readBuf);
                             Console.WriteLine(str);

                             byte[] device1 = new byte[6];
                             byte[] device2 = new byte[6];
                             Array.Copy(readBuf, 15, device1, 0, 6);
                             Array.Copy(readBuf, 21, device2, 0, 6);
                             string deviceStr1 = ByteArrayToString(device1);
                             string deviceStr2 = ByteArrayToString(device2);
                             Console.WriteLine("Channel 1 id is " + deviceStr1);
                             Console.WriteLine("Channel 2 id is " + deviceStr2);

                             break;
                         }
                     }

                     while (true)
                     {
                         await Task.Delay(500);
                         s.Write(rgbbuf);
                         s.Write(rgbbuf2);
                         s.Write(rgbbuf3);

                     }
                 });

             }
             else
             {
                 Console.WriteLine("cant write");
             }
             Console.Read();
             RestartNZXT();

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

        /*public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2} ", b);
            return hex.ToString();
        }*/
    }
}

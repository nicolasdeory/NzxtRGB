using HidSharp;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace NzxtRGB
{
    public abstract class NzxtDevice : IDisposable
    {
        NzxtDeviceConfig config;

        protected virtual int VendorId { get; }
        protected virtual int ProductId { get; }

        protected HidStream DeviceStream;

        private string nzxtCamPath;

        protected NzxtDevice()
        {
            config = new NzxtDeviceConfig();
        }

        protected NzxtDevice(NzxtDeviceConfig config)
        {
            if (config == null)
                this.config = new NzxtDeviceConfig();
            else
                this.config = config;
        }

        private void CloseNzxtCam()
        {
            Process[] processes = Process.GetProcessesByName("NZXT CAM");
            Process nzxtCam = processes[0];
            nzxtCamPath = nzxtCam.MainModule.FileName;
            nzxtCam.Kill();
        }

        private void OpenNzxtCam()
        {
            if (nzxtCamPath == null)
                throw new InvalidOperationException("Couldn't find Nzxt CAM executable");
            Process.Start(nzxtCamPath, "--startup");
        }

        private static void RestartNZXT()
        {
            Process[] processes = Process.GetProcessesByName("NZXT CAM");
            Process nzxtCam = processes[0];
            string path = nzxtCam.MainModule.FileName;
            nzxtCam.Kill();
            Process.Start(path, "--startup");
        }

        protected async Task<bool> OpenHidDevice()
        {
            HidDevice hid = DeviceList.Local.GetHidDeviceOrNull(VendorId, ProductId);
            if (hid == null)
                return false;

            if (!hid.TryOpen(out DeviceStream))
            {
                if (config.RestartNzxtCamOnFail)
                {
                    CloseNzxtCam();
                    await Task.Delay(500).ConfigureAwait(false);
                    DeviceStream = hid.Open();
                    OpenNzxtCam();
                }
                else
                {
                    throw new InvalidOperationException("Couldn't open the device. Try closing NZXT Cam or enable RestartNzxtCamOnFail option");
                }

            }
            if (DeviceStream.CanWrite)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Closes the device
        /// </summary>
        public void Dispose()
        {
            DeviceStream.Dispose();
            if (config.RestartNzxtOnClose)
            {
                RestartNZXT();
            }

        }
    }
}

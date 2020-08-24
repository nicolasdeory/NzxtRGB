# NzxtRGB
You can install the library via [NuGet](https://www.nuget.org/packages/NzxtRGB/).

### Supported devices
Only the Smart Device V2 is supported for now. The library was tested with Aer 2 fans and a Strip V2 connected. You might get it working with other devices, but you won't have information about led count or connected device.

### Getting started

Check the sample project, or use this sample code:

```csharp
NzxtDeviceConfig config = new NzxtDeviceConfig();
config.RestartNzxtCamOnFail = true;
config.RestartNzxtOnClose = true;
SmartDeviceV2 device = await SmartDeviceV2.OpenDeviceAsync(config);
Debug.WriteLine("Device on Channel 1: " + device.Channel1);
Debug.WriteLine("Device on Channel 2: " + device.Channel2);

List<Color> ch1Colors = new List<Color>();
for(int i = 0; i < device.Channel1.LedCount; i++)
{
    ch1Colors.Add(Color.FromArgb(255, 0, 0)); // Red color
}

device.SendRGB(1, ch1Colors.ToArray());
device.Dispose(); // Clean up once you're done, to close the device and restart NZXT Cam when applicable

```

### Config
You can pass a `NzxtDeviceConfig` object when you're opening the device.
There are two options available:
- `RestartNzxtCamOnFail`: If the device is in use by NZXT Cam (which is very probable if you open the software before your app), the library will restart NZXT Cam automatically to force the HID device into shared mode.
- `RestartNzxtOnClose`: No effect unless `RestartNzxtCamOnFail` is enabled. When the device is closed, the library will restart NZXT Cam so it stops complaining about another app controlling the lighting. You should only enable it on one device, if you're using multiple, so only one restart is triggered after all the devices are closed.

### Some considerations

If you're running NZXT Cam, the device will be in shared mode, and NZXT Cam will send updates periodically. So if you want to keep the custom color, you'll need to send updates periodically (every 500ms is fine).

Make sure not to send updates too fast (stick to 30fps). We're not responsible for any damage done to hardware.

### Contributing
Contributions are welcome. Feel free to test this with your own setup and add your own devices. You can also add an issue with HID packets with a program such as Free USB Analyzer so support for that device can be added.

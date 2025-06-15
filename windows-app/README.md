# Airpod Battery Monitor for Windows (WPF)

This directory contains a proof-of-concept WPF application that replicates the core functionality of the GNOME extension. It listens for BLE advertisements from AirPods/Beats and shows their battery information.

The project targets .NET 6.0 and requires the `Windows` SDK for the `Windows.Devices.Bluetooth` namespaces.

To build on Windows:

```bash
# from the windows-app directory
# requires .NET SDK installed
 dotnet build
```

The app starts a `BluetoothLEAdvertisementWatcher` and updates the UI when supported Apple devices are detected.

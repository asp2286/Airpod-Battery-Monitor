using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Storage.Streams;

namespace AirpodBatteryMonitorWindows
{
    public class BluetoothService
    {
        private const ushort AppleManufacturerId = 0x004C;
        private const int ManufacturerDataLength = 27;
        private readonly BluetoothLEAdvertisementWatcher _watcher;

        public ObservableCollection<DeviceInfo> Devices { get; } = new();

        public BluetoothService()
        {
            _watcher = new BluetoothLEAdvertisementWatcher();
            _watcher.Received += OnAdvertisementReceived;
        }

        public void Start() => _watcher.Start();

        private void OnAdvertisementReceived(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementReceivedEventArgs args)
        {
            var manufacturer = args.Advertisement.ManufacturerData.FirstOrDefault(m => m.CompanyId == AppleManufacturerId);
            if (manufacturer == null) return;

            var data = new byte[manufacturer.Data.Length];
            DataReader.FromBuffer(manufacturer.Data).ReadBytes(data);
            if (data.Length != ManufacturerDataLength) return;

            if (data[0] != 0x07) return;
            if (data[1] != 0x19 && data[1] != 0x13) return;

            var hex = BitConverter.ToString(data).Replace("-", string.Empty).ToLowerInvariant();
            var model = hex.Substring(6, 4);
            var decoded = DecodeManufacturerData(hex);

            App.Current.Dispatcher.Invoke(() =>
            {
                var existing = Devices.FirstOrDefault(d => d.Model == model);
                if (existing == null)
                    Devices.Add(new DeviceInfo { Model = model, Data = decoded });
                else
                    existing.Data = decoded;
            });
        }

        private DeviceData DecodeManufacturerData(string hex)
        {
            bool flip = (Convert.ToInt32(hex.Substring(10, 1), 16) & 0x02) == 0;
            int LeftLevel() => CorrectLevel(Convert.ToInt32(hex.Substring(flip ? 12 : 13, 1), 16));
            int RightLevel() => CorrectLevel(Convert.ToInt32(hex.Substring(flip ? 13 : 12, 1), 16));
            int CaseLevel() => CorrectLevel(Convert.ToInt32(hex.Substring(15, 1), 16));
            int SingleLevel() => CorrectLevel(Convert.ToInt32(hex.Substring(13, 1), 16));

            int chargeStatus = Convert.ToInt32(hex.Substring(14, 1), 16);
            bool LeftStatus() => (chargeStatus & (flip ? 0b00000010 : 0b00000001)) != 0;
            bool RightStatus() => (chargeStatus & (flip ? 0b00000001 : 0b00000010)) != 0;
            bool CaseStatus() => (chargeStatus & 0b00000100) != 0;
            bool SingleStatus() => (chargeStatus & 0b00000001) != 0;

            return new DeviceData
            {
                LeftLevel = LeftLevel(),
                RightLevel = RightLevel(),
                CaseLevel = CaseLevel(),
                SingleLevel = SingleLevel(),
                LeftStatus = LeftStatus(),
                RightStatus = RightStatus(),
                CaseStatus = CaseStatus(),
                SingleStatus = SingleStatus(),
                UpdatedTime = DateTime.Now.ToString("T")
            };
        }

        private static int CorrectLevel(int value)
        {
            return value switch
            {
                15 => -1,
                0 => 5,
                _ => value > 9 ? 100 : value * 10,
            };
        }
    }

    public class DeviceInfo
    {
        public string Model { get; set; } = string.Empty;
        public DeviceData Data { get; set; } = new DeviceData();
    }

    public class DeviceData
    {
        public int LeftLevel { get; set; }
        public int RightLevel { get; set; }
        public int CaseLevel { get; set; }
        public int SingleLevel { get; set; }
        public bool LeftStatus { get; set; }
        public bool RightStatus { get; set; }
        public bool CaseStatus { get; set; }
        public bool SingleStatus { get; set; }
        public string UpdatedTime { get; set; } = string.Empty;
    }
}


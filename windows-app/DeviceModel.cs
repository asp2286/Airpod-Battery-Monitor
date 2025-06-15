namespace AirpodBatteryMonitorWindows
{
    public record DeviceModel(string Key, string Text);

    public static class DeviceModels
    {
        public static readonly DeviceModel[] List = new[]
        {
            new DeviceModel("-1", "Unknown"),
            new DeviceModel("0220", "AirPods 1st Gen"),
            new DeviceModel("0F20", "AirPods 2nd Gen"),
            new DeviceModel("1320", "AirPods 3rd Gen"),
            new DeviceModel("1920", "AirPods 4th Gen"),
            new DeviceModel("1B20", "AirPods 4th Gen with ANC"),
            new DeviceModel("0E20", "AirPods Pro"),
            new DeviceModel("1420", "AirPods Pro 2nd Gen"),
            new DeviceModel("2420", "AirPods Pro 2nd Gen USB-C"),
            new DeviceModel("1220", "Beats Fit Pro"),
            new DeviceModel("0520", "Beats X"),
            new DeviceModel("1020", "Beats Flex"),
            new DeviceModel("0620", "Beats Solo 3"),
            new DeviceModel("0320", "Powerbeats 3"),
            new DeviceModel("0920", "Beats Studio 3"),
            new DeviceModel("0A20", "AirPods Max"),
            new DeviceModel("1F20", "AirPods Max USB-C"),
            new DeviceModel("0B20", "Powerbeats Pro"),
            new DeviceModel("0C20", "Beats Solo Pro"),
            new DeviceModel("0D20", "Powerbeats 4"),
            new DeviceModel("1720", "Beats Studio Pro"),
            new DeviceModel("1120", "Beats Studio Buds"),
            new DeviceModel("1620", "Beats Studio Buds Plus"),
        };
    }
}


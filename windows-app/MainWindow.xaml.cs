using System.Windows;

namespace AirpodBatteryMonitorWindows
{
    public partial class MainWindow : Window
    {
        private readonly BluetoothService _bluetoothService;

        public MainWindow()
        {
            InitializeComponent();
            _bluetoothService = new BluetoothService();
            DataContext = _bluetoothService;
            _bluetoothService.Start();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.ObjectModel;
namespace WpfPractice
{
    public class MainWindowViewModel : BindableBase
    {
        private string _deviceName = "模拟设备 A";

        private string _deviceLocation = "二号车间";
        public ObservableCollection<DeviceRecord> LocationHistory { get; } =
            new ObservableCollection<DeviceRecord>();
        public ObservableCollection<DeviceNode> DeviceRoots { get; } =
    new ObservableCollection<DeviceNode>();

        private DeviceRecord? _selectedRecord;
        private DeviceNode? _viewedNode;

        public DeviceNode? ViewedNode
        {
            get
            {
                return _viewedNode;
            }
            set
            {
                SetProperty(ref _viewedNode, value);
            }
        }

        public DelegateCommand<DeviceNode> ShowNodeCommand { get; }

        private void ShowNode(DeviceNode? node)
        {
            ViewedNode = node;
        }

        public DeviceRecord? SelectedRecord
        {
            get
            {
                return _selectedRecord;
            }
            set
            {
                SetProperty(ref _selectedRecord, value);
            }
        }
        public string DeviceLocation
        {
            get
            {
                return _deviceLocation;
            }
            set
            {
                SetProperty(ref _deviceLocation, value);
              
            }
        }
        public string DeviceName
        {
            get
            {
                return _deviceName;
            }
            set
            {
                SetProperty(ref _deviceName ,value);

                // 通知绑定：DeviceName 已经变化，请重新读取。
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DeviceName)));
            }
        }
        public void ResetLocation()
        {
            DeviceRecord record = new DeviceRecord
            {
                Name = DeviceName,
                Location = DeviceLocation
            };

            LocationHistory.Add(record);

            DeviceLocation = "三号车间";
        }
        public void DeleteLocation(DeviceRecord? record)
        {
            if (record == null)
                return;
            LocationHistory.Remove(record);
            SelectedRecord = null;

        }
        public DelegateCommand ResetLocationCommand { get; }
        public DelegateCommand<DeviceRecord> RestoreLocationCommand { get; }
        public DelegateCommand<DeviceRecord> DeleteLocationCommand { get; }
      
        private void RestoreLocation(DeviceRecord? record)
        {


            if (record == null)
            {
                return;
            }

            DeviceLocation = record.Location;
        }
   
        private bool CanRestoreLocation(DeviceRecord? record)
        {
            return record != null;
        }
        private bool CanDeleteLocation(DeviceRecord? record)
        {
            return record != null;
        }

        public MainWindowViewModel(IDeviceTreeService treeService)
        {
            ResetLocationCommand = new DelegateCommand(ResetLocation, CanResetLocation).ObservesProperty(()=>DeviceLocation);
            RestoreLocationCommand =new DelegateCommand<DeviceRecord>(RestoreLocation, CanRestoreLocation).ObservesProperty(() => SelectedRecord);
            DeleteLocationCommand = new DelegateCommand<DeviceRecord>(DeleteLocation, CanDeleteLocation).ObservesProperty(() => SelectedRecord);
            ShowNodeCommand = new DelegateCommand<DeviceNode>(ShowNode);
            DeviceRoots.Add(treeService.CreateWorkshop());
        }

        private bool CanResetLocation() {
            return DeviceLocation != "三号车间";
        }
       
    }
   

}

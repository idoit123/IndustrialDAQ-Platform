using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfPractice;

public class DeviceTreeService : IDeviceTreeService
{
    public DeviceNode CreateWorkshop()
    {
        DeviceNode workshop = new DeviceNode
        {
            Name = "一号车间"
        };

        DeviceNode deviceA = new DeviceNode
        {
            Name = "模拟设备 A"
        };

        deviceA.Children.Add(new DeviceNode
        {
            Name = "温度测点"
        });

        deviceA.Children.Add(new DeviceNode
        {
            Name = "压力测点"
        });

        workshop.Children.Add(deviceA);

        workshop.Children.Add(new DeviceNode
        {
            Name = "模拟设备 B"
        });

        return workshop;
    }
}

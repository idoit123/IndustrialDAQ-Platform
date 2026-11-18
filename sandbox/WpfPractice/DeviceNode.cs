using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.ObjectModel;

namespace WpfPractice;

public class DeviceNode
{
    public string Name { get; set; } = "";

    public ObservableCollection<DeviceNode> Children { get; } =
        new ObservableCollection<DeviceNode>();
}

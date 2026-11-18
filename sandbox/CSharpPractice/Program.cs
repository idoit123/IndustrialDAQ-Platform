using System;
using System.Threading.Tasks;

// 外部先创建驱动。
IProtocolDriver driver = new ModbusDriver();

// 创建采集器，把驱动传进去。
DeviceCollector collector = new DeviceCollector(driver);

// 让采集器执行一次读取。
await collector.CollectOnceAsync();


public interface IProtocolDriver
{
    Task ReadAsync();
}


// 教学模拟：等待一秒并打印消息，不连接真实设备。
public class ModbusDriver : IProtocolDriver
{
    public async Task ReadAsync()
    {
        Console.WriteLine("Modbus 驱动：开始读取");

        await Task.Delay(1000);

        Console.WriteLine("Modbus 驱动：读取完成");
    }
}


public class DeviceCollector
{
    private readonly IProtocolDriver _driver;

    public DeviceCollector(IProtocolDriver driver)
    {
        Console.WriteLine("采集器：保存传入的驱动");

        _driver = driver;
    }

    public async Task CollectOnceAsync()
    {
        Console.WriteLine("采集器：准备采集");

        await _driver.ReadAsync();

        Console.WriteLine("采集器：本轮采集完成");
    }
}
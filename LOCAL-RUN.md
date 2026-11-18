# 本机无硬件运行

本机已安装 .NET SDK 8.0.428（含 WPF 运行时）和 Python 3.12.10。
Modbus 模拟器依赖安装在项目的 `.venv` 中，版本固定于
`config/requirements-simulator.txt`。

## 启动

双击项目根目录的 `start-local.cmd`。

脚本会启动本机 Modbus 模拟器和 WPF 客户端。已有的本机 502 端口服务会被复用，
因此请确保该端口运行的是本项目模拟器。
修改 C# 或 XAML 后，在项目目录执行 `./start-local.cmd -Rebuild`；执行前先关闭客户端。

本机演示配置从 `config/production-line.json` 提取本机 Modbus 设备及测点，
生成到 `src/IndustrialDAQ.UI/bin/Debug/net8.0-windows/config/production-line.json`。
客户端优先读取此生成目录，因此演示时不会尝试连接原配置中的外部 PLC。
要演示设备配置热更新，可编辑该生成文件；下次通过启动脚本新开客户端时会重新生成。

首次登录账号：`admin`，初始密码：`Admin#2026`。
客户端默认使用 SQLite，无需安装数据库服务器。
关闭客户端后，模拟器会继续在后台运行。需要停止时，用任务管理器结束
`artifacts/local-run/simulator.pid` 所记录的进程；停止前核对其为本项目的 Python 模拟器。

## 构建与测试

### 使用 Visual Studio 开发

本机已安装 Visual Studio Community 2022 **17.11.6**，包含“.NET 桌面开发”和 Blend 组件。
本机 Windows 10 IoT Enterprise LTSC 2021 的补丁版本为 19044.1288，
原有 VS 2026 和新版 VS 2022 17.14.45 的 .NET 10 后台服务无法正常启动，
因此按用户选择改用后台服务基于 .NET 8 的 17.11.6，没有安装 Windows 系统更新。
原有 VS 2026 保留，本次新增的 17.14.45 已卸载。

双击根目录的 `open-visual-studio.cmd`，使用 Visual Studio 2022 17.11.x 打开解决方案；
也可在该版本 Visual Studio 中直接打开 `Projects.sln`。
首次启动时，需自行登录或选择“暂时跳过此项”，并完成首次设置。
当前机器请保持此版本，升级 Visual Studio 前先确认系统兼容性。

在解决方案资源管理器中右键 `IndustrialDAQ.UI`，选择“设为启动项目”，然后按 F5 调试。
按 F9 设置或取消断点，按 F10 单步执行，按 F11 进入方法。

本机 Modbus 模拟器需要保持运行。如果模拟器尚未启动，先运行 `start-local.cmd`，
再关闭它打开的上位机窗口；模拟器会继续运行。之后在 Visual Studio 中按 F5 启动上位机。
调试前关闭已有上位机，避免生成文件被占用。

根目录 `global.json` 将本项目固定到已验证的 .NET 8 SDK：
这台 Windows 上，.NET 10 编译器报告 CET 兼容性错误；项目继续使用已经验证的
SDK 8.0.428，并允许同功能带内的补丁更新。开发入口同时固定为 VS 2022 17.11.x，
避免启动依赖 .NET 10 后台服务的新版本 IDE。

重新安装旧版时，可从[微软维护的 WinGet 17.11.6 清单](https://github.com/microsoft/winget-pkgs/blob/master/manifests/m/Microsoft/VisualStudio/2022/Community/17.11.6/Microsoft.VisualStudio.2022.Community.installer.yaml)
获取固定版本的官方安装包地址及 SHA256。本次安装已验证哈希和微软数字签名。

```powershell
dotnet build Projects.sln
dotnet test tests/IndustrialDAQ.Tests/IndustrialDAQ.Tests.csproj --no-build
```

.NET 8 SDK 可编译本项目；当前构建存在 OpenCV 分析器编译器版本提示及少量可空性警告。
海康 SDK 相关测试在未安装对应 SDK 时直接返回，通过数不代表已完成海康硬件验证。

## 日志与数据

- 启动和构建日志：`artifacts/local-run/`
- SQLite：`src/IndustrialDAQ.UI/bin/Debug/net8.0-windows/industrialdaq.db`
- 用户级设置：`%LOCALAPPDATA%/IndustrialDAQ/`

若需重新创建 Python 环境：

```powershell
python -m venv .venv
./.venv/Scripts/python.exe -m pip install -r config/requirements-simulator.txt
```

首次安装 Python 后，已有终端可能需要重新打开，才能识别新的 `python` 命令。

# WPF / Prism 学习安排

每三天验收，每天 4～5 小时。不单独安排 C# 语法课，遇到影响理解的写法就地解释。
每个知识点都采用“独立手写练习 + 工业项目对应代码”的方式学习，不要求先学完框架再看项目。每次主要学习一个概念，并说明控件、变量和参数的来源。
练习源码由学习者手写；助手读取、解释和检查，未经请求不代写修改。

## 第一轮：窗口、控件、布局和事件

- 第 1 天：Window、XAML 与后台代码的关系；x:Class、x:Name、InitializeComponent；按钮点击事件。
- 第 2 天：Grid、StackPanel、行列、Margin/Padding、尺寸和对齐；TextBlock、TextBox、Button、ComboBox。
- 第 3 天：组合输入、按钮和列表做小窗口；练习事件处理与断点调试。
- 验收：独立制作一个能输入名称、添加记录并显示列表的窗口，解释控件在哪里创建、事件由谁调用。

## 第二轮：绑定与 MVVM

- 第 4 天：Binding、DataContext、绑定路径、双向绑定、UpdateSourceTrigger。
- 第 5 天：属性变更通知、ObservableCollection、ItemsSource、DataGrid、SelectedItem。
- 第 6 天：View / ViewModel / Model、ICommand；用命令和绑定整理练习程序。
- 验收：输入、列表、选中项和按钮状态能够联动，解释数据怎样到达界面。

## 第三轮：项目采用的 Prism 基础

- 第 7 天：BindableBase / SetProperty 与 DelegateCommand，理解它们替代的重复代码。
- 第 8 天：构造函数依赖、注册与解析、单例与按次创建、ViewModelLocator；先说明手动创建过程，再使用容器。
- 第 9 天：PrismApplication、CreateShell、RegisterTypes；组织一个简单应用。
- 验收：用 Prism 组织练习，能够追踪窗口、ViewModel 和依赖对象的来源。

## 第四轮：项目中的页面交互与界面配套

- 第 10 天：Region、注册页面、RequestNavigate、导航参数与 INavigationAware。
- 第 11 天：IDialogService / IDialogAware；IEventAggregator / PubSubEvent，比较直接调用与事件通信。
- 第 12 天：按练习需要引入资源与样式、转换器、模板；认识 UI 线程与 Dispatcher。
- 验收：两个页面可以切换，有一个编辑对话框，页面之间能传递一次更新通知；模拟数据更新不会阻塞界面。

第 13～14 天用于补漏和综合修改。自定义控件、依赖属性、图表和主题模板需要结合实际功能学习；先掌握能读懂和修改项目现有用法的部分。

## 第一课实验

在 Visual Studio 中按 Ctrl+Shift+O，打开本目录的 WpfPractice.sln，再按 Ctrl+F5。
点击按钮后，文字应更新为点击次数。

先认识两个文件：

- MainWindow.xaml：声明窗口、布局和控件。
- MainWindow.xaml.cs：同一个窗口的 C# 部分，初始化控件并处理点击事件。

本次练习：只把 Button 的 Content 从“点我”改成“开始练习”，保存后关闭原窗口并重新运行，观察按钮文字。

## 工业项目中的对照位置（学完相应知识点后再看）

- src/IndustrialDAQ.UI/MainWindow.xaml：窗口布局、资源、样式、命令、Prism 内容区域。
- src/IndustrialDAQ.UI/MainWindow.xaml.cs：窗口初始化、DataContext 和窗口事件。
- src/IndustrialDAQ.UI/Views/ProductionMonitorView.xaml：下拉框、数据表格、绑定和数据模板。
- src/IndustrialDAQ.UI/ViewModels/ProductionMonitorViewModel.cs：BindableBase、SetProperty、集合、命令、事件和对话框。
- src/IndustrialDAQ.UI/App.xaml.cs：Prism 启动、服务注册、页面注册和初始导航。
- src/IndustrialDAQ.UI/Converters：值转换器。
- src/IndustrialDAQ.UI/Themes：主题资源和控件样式。
- src/IndustrialDAQ.UI/Events：Prism 事件类型。

当前工业 UI 项目声明使用 .NET 8 WPF 和 Prism.DryIoc 9.0.537。Prism 课程以该版本的实际接口为准。

## 项目覆盖核对

本清单覆盖目标是读懂并修改本项目的 WPF / Prism 界面，不等于完成整个 SCADA 平台。日期是安排参考，是否进入下一步以手写与实际运行验收为准。

已经手写并通过编译检查的部分：基本窗口和网格布局、点击事件、绑定数据来源、属性变化通知、双向绑定、数据移入独立 ViewModel、DelegateCommand、CanExecute 与执行状态刷新。编译检查不替代界面运行验收。

后续必须覆盖的界面用法：

- [ ] Prism BindableBase / SetProperty；带参数命令和 CommandParameter；ObservesProperty。验收：选择一个数据项后，命令接收该项并正确启用或禁用。对照 [生产监控 ViewModel](D:/Projects/IndustrialDAQ-master/src/IndustrialDAQ.UI/ViewModels/ProductionMonitorViewModel.cs:85)。
- [ ] ObservableCollection、ItemsSource、SelectedItem、ComboBox、DataGrid；集合变化与单个项目属性变化的区别。验收：增加、删除、选择、修改记录时界面正确更新。对照 [生产监控页面](D:/Projects/IndustrialDAQ-master/src/IndustrialDAQ.UI/Views/ProductionMonitorView.xaml:182)。
- [ ] TreeView、Children、HierarchicalDataTemplate 与模板内部绑定来源。验收：显示动态层级资源，操作选中节点。对照 [设备详情页](D:/Projects/IndustrialDAQ-master/src/IndustrialDAQ.UI/Views/DeviceDetailView.xaml:65)。
- [ ] PrismApplication、CreateShell、RegisterTypes、构造函数依赖、注册与解析、对象生命周期、ViewModelLocator；按实际代码区分手动设置和框架关联 DataContext。验收：能指出窗口及 ViewModel 的创建位置和数据来源。对照 [应用启动](D:/Projects/IndustrialDAQ-master/src/IndustrialDAQ.UI/App.xaml.cs:54)。
- [ ] Region、RegisterForNavigation、RequestNavigate、导航参数、INavigationAware。验收：两个页面切换并传递选中记录。
- [ ] IDialogService / IDialogAware、参数与结果回传、关闭流程。验收：通过对话框编辑一项数据并返回结果。对照 [写入对话框 ViewModel](D:/Projects/IndustrialDAQ-master/src/IndustrialDAQ.UI/ViewModels/WriteTagDialogViewModel.cs)。
- [ ] IEventAggregator / PubSubEvent、订阅和取消订阅、IDestructible、取消令牌与释放资源；检查现有代码的生命周期边界。验收：重复进入退出页面后，不留下重复订阅和后台任务。
- [ ] 后台实时数据到 ViewModel、Dispatcher 与 UI 线程、页面停止时取消读取。验收：模拟数据连续更新且窗口能响应操作。对照 [实时订阅循环](D:/Projects/IndustrialDAQ-master/src/IndustrialDAQ.UI/ViewModels/ProductionMonitorViewModel.cs:290)。
- [ ] ResourceDictionary、StaticResource / DynamicResource、Style、Trigger、DataTemplate / ControlTemplate、值转换器。验收：根据状态显示颜色或可见性，并解释资源来源。
- [ ] 自定义控件与 DependencyProperty、LiveCharts 实时曲线和历史数据展示。验收：把模拟值绑定到仪表和趋势图；能定位现有图表的系列及更新入口。对照 [仪表依赖属性](D:/Projects/IndustrialDAQ-master/src/IndustrialDAQ.UI/Controls/SemiCircleGauge.xaml.cs:18) 和 [趋势 ViewModel](D:/Projects/IndustrialDAQ-master/src/IndustrialDAQ.UI/ViewModels/TrendViewModel.cs)。

这些是项目相关用法，不要求穷尽整个 WPF 或 Prism 的所有 API。

## 完整 SCADA 的另一条学习线

框架练习与以下实际业务链路穿插，不单独开泛泛的 C# 语法课程；遇到必要的异步、集合或数据库机制时结合代码解释。

- [ ] 设备配置 → DriverFactory → 协议驱动 → DeviceCollector；连接、读写、重试、取消和配置重载。
- [ ] AcquisitionChannel → HistoryWriter → 实时缓存与历史持久化；队列与广播的区别、批量、死区、重试和失败回放。
- [ ] 实时值 → 规则引擎 → 报警状态机 → 报警中心；规则与状态迁移，触发、恢复、确认的区别。
- [ ] ResourcePath / 资源树 → 动态授权 → 写入、确认等业务操作及审计。命令禁用只是界面反馈，不能代替业务授权。
- [ ] 运行时配置加载与热更新；保存、加载、刷新及当前删除行为的实际边界。
- [ ] 数据加工、虚拟测点与视觉模块，作为基础采集、报警和授权链路之后的扩展。

综合验收：用本机模拟设备跟踪一个测点，从配置和读取到实时显示、历史存储、规则判断与授权操作；独立完成一个小修改，说明输入、输出、对象来源及验证结果。没有硬件的协议或视觉能力明确标注为源码学习或模拟验证。

using System.Windows;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Navigation.Regions;

namespace WpfPractice;

public partial class App : PrismApplication
{
    protected override void RegisterTypes(
        IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterSingleton<IDeviceTreeService, DeviceTreeService>();
        containerRegistry.Register<MainWindowViewModel>();
        containerRegistry.Register<MainWindow>();
        containerRegistry.RegisterForNavigation<DevicePage>();
        ViewModelLocationProvider.Register<MainWindow,MainWindowViewModel>();
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        IRegionManager regionManager =
            Container.Resolve<IRegionManager>();

        regionManager.RequestNavigate(
            "PracticeRegion",
            nameof(DevicePage));
    }
    protected override Window CreateShell()
    {
        return Container.Resolve<MainWindow>();
    }
}
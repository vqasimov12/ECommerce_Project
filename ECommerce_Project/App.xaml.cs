using ECommerce_Project.Views.CommonViews;
using System.Windows;
using SimpleInjector;
using ECommerce_Project.ViewModels.CommonViewModels;
using ECommerce_Project.Entity.Models;
using ECommerce_Project.Views.UserViews;
using ECommerce_Project.Views.AdminViews;
using ECommerce_Project.ViewModels.AdminViewModels;
using ECommerce_Project.ViewModels.UserViewModels;
using Syncfusion.Licensing;

namespace ECommerce_Project;
public partial class App : Application
{
    public static Container Container { get; private set; } = new();
    public App()
    {
        SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NCaF1cWWhAYVF+WmFZfVpgdVRMYlVbQH5PIiBoS35RckVrW3Zcc3BWQ2NVVkN1");
        Container.RegisterSingleton<MainWindow>();
        Container.RegisterSingleton<AppDataContext>();
        RegisterViews();
        RegisterViewModels();
    }

    void RegisterViews()
    {
        Container.RegisterSingleton<AdminDashboardPageView>();
        Container.RegisterSingleton<AdminCategoryPageView>();
        Container.RegisterSingleton<AdminProductPageView>();
        Container.RegisterSingleton<AdminCustomersPageView>();
        Container.RegisterSingleton<AdminHomePageView>();
        Container.RegisterSingleton<AdminOrderPageView>();

        Container.RegisterSingleton<LoginPageView>();

        Container.RegisterSingleton<SignUpPageView>();
        Container.RegisterSingleton<UserDashboardPageView>();
        Container.RegisterSingleton<UserHomePageView>();
        Container.RegisterSingleton<UserShoppingCartPageView>();
        Container.RegisterSingleton<UserFavouritesPageView>();
        Container.RegisterSingleton<UserOrderPageView>();
        Container.RegisterSingleton<UserProfilePageView>();
        Container.RegisterSingleton<UserPaymentPageView>();

    }
    void RegisterViewModels()
    {
        Container.RegisterSingleton<AdminDashboardPageViewModel>();
        Container.RegisterSingleton<AdminCategoryPageViewModel>();
        Container.RegisterSingleton<AdminProductPageViewModel>();
        Container.RegisterSingleton<AdminCustomersPageViewModel>();
        Container.RegisterSingleton<AdminHomePageViewModel>();
        Container.RegisterSingleton<AdminOrderPageViewModel>();
        Container.RegisterSingleton<AdminEditProductImagesWindowViewModel>();

        Container.RegisterSingleton<LoginPageViewModel>();

        Container.RegisterSingleton<SignUpPageViewModel>();
        Container.RegisterSingleton<UserDasboardPageViewModel>();
        Container.RegisterSingleton<UserHomePageViewModel>();
        Container.RegisterSingleton<UserShoppingCartPageViewModel>();
        Container.RegisterSingleton<UserFavouritesPageViewModel>();
        Container.RegisterSingleton<UserOrderPageViewModel>();
        Container.RegisterSingleton<UserProfilePageViewModel>();
        Container.RegisterSingleton<UserPaymentPageViewModel>();
    }
    protected override void OnStartup(StartupEventArgs e)
    {
        var view = Container.GetInstance<MainWindow>();
        var page = Container.GetInstance<LoginPageView>();
        page.DataContext = Container.GetInstance<LoginPageViewModel>();
        view.Navigate(page);
        view.ShowDialog();
        base.OnStartup(e);
        Current.Shutdown();
    }
}


using ECommerce_Project.Views.CommonViews;
using System.Windows;
using SimpleInjector;
using ECommerce_Project.ViewModels.CommonViewModels;
using ECommerce_Project.Entity.Models;
using ECommerce_Project.Views.UserViews;
using ECommerce_Project.Views.AdminViews;
using ECommerce_Project.ViewModels.AdminViewModels;
using ECommerce_Project.ViewModels.UserViewModels;

namespace ECommerce_Project;
public partial class App : Application
{
    public static Container Container { get; private set; } = new();
    public App()
    {

        Container.RegisterSingleton<MainWindow>();
        Container.RegisterSingleton<AppDataContext>();
        RegisterViews();
        RegisterViewModels();
    }

    void RegisterViews()
    {
        Container.RegisterSingleton<LoginPageView>();
        Container.RegisterSingleton<SignUpPageView>();
        Container.RegisterSingleton<AdminDashboardPageView>();
        Container.RegisterSingleton<AdminCategoryPageView>();
        Container.RegisterSingleton<AdminProductPageView>();
        Container.RegisterSingleton<AdminCustomersPageView>();
        Container.RegisterSingleton<AdminHomePageView>();
        Container.RegisterSingleton<UserDashboardPageView>();
        
    }
    void RegisterViewModels()
    {
        Container.RegisterSingleton<LoginPageViewModel>();
        Container.RegisterSingleton<SignUpPageViewModel>();
        Container.RegisterSingleton<AdminDashboardPageViewModel>();
        Container.RegisterSingleton<AdminCategoryPageViewModel>();
        Container.RegisterSingleton<AdminProductPageViewModel>();
        Container.RegisterSingleton<AdminCustomersPageViewModel>();
        Container.RegisterSingleton<AdminHomePageViewModel>();
        Container.RegisterSingleton<UserDasboardPageViewModel>();

    }
    protected override void OnStartup(StartupEventArgs e)
    {
        var view = Container.GetInstance<MainWindow>();
        var page=Container.GetInstance<LoginPageView>();
        page.DataContext = Container.GetInstance<LoginPageViewModel>();
        view.Navigate(page);
        view.ShowDialog();
        base.OnStartup(e);
        Current.Shutdown();
    }
}


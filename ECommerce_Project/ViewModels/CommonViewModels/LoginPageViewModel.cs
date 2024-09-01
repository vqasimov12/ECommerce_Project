using ECommerce_Project.Command;
using ECommerce_Project.Entity.Models;
using ECommerce_Project.ViewModels.AdminViewModels;
using ECommerce_Project.ViewModels.UserViewModels;
using ECommerce_Project.Views.AdminViews;
using ECommerce_Project.Views.CommonViews;
using ECommerce_Project.Views.UserViews;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ECommerce_Project.ViewModels.CommonViewModels;
public class LoginPageViewModel : BaseViewModel
{
    private string? email;
    private string? password;

    public string? Email { get => email; set { email = value; OnPropertyChanged(); } }
    public string? Password { get => password; set { password = value; OnPropertyChanged(); } }
    public LoginPageViewModel()
    {
        SignUpCommand = new RelayCommand(SignUpCommandExcute);
        LoginCommand = new RelayCommand(loginCommandExecute);
    }

    #region SignUpCommand

    public ICommand SignUpCommand { get; set; }
    public void SignUpCommandExcute(object? obj)
    {
        if (obj is Page page)
        {
            var view = App.Container.GetInstance<SignUpPageView>();
            view.DataContext = App.Container.GetInstance<SignUpPageViewModel>();
            page.NavigationService.Navigate(view);
        }
    }
    #endregion

    #region LoginCommand    
    public ICommand LoginCommand { get; set; }
    public void loginCommandExecute(object? obj)
    {
        //if (Email == "admin@gmail.com")
        //{
        //    if (Password == "admin123")
        //    {
        var page = obj as Page;
        if (page is null) return;
        var window = Window.GetWindow(page);
        if (window is null) return;
        window.ResizeMode = window.ResizeMode | ResizeMode.CanResize;
        window.WindowStyle = WindowStyle.SingleBorderWindow;
        window.Width = 1070;
        window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        window.Height = 630;
        var _page = App.Container.GetInstance<AdminDashboardPageView>();
        var datacontext = App.Container.GetInstance<AdminDashboardPageViewModel>();
        _page.DataContext = datacontext;
        var currentPage = App.Container.GetInstance<AdminHomePageView>();
        var data = App.Container.GetInstance<AdminHomePageViewModel>();
        data.RefreshDataSource();
        currentPage.DataContext = data;
        datacontext.CurrentPage = currentPage;
        if (datacontext.PreviouslySelectedPanel != null)
            datacontext.PreviouslySelectedPanel.Tag = null;
        var stackPanel = _page.homePanel;
        stackPanel.Tag = "Selected";
        datacontext.PreviouslySelectedPanel = stackPanel;
        page.NavigationService.Navigate(_page);
        return;
        //    }
        //    else
        //    {
        //        MessageBox.Show("Incorrect Password", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //        return;
        //    }

        //}



        //var user = CheckEmail(Email);
        //if (user is not null)
        //    if (user.Password == Password)
        //    {
        //        using var db = new AppDataContext();
        //        var page = obj as Page;
        //        if (page is null) return;
        //        var window = Window.GetWindow(page);
        //        if (window is null) return;
        //        window.ResizeMode = window.ResizeMode | ResizeMode.CanResize;
        //        window.WindowStyle = WindowStyle.SingleBorderWindow;
        //        window.Width = 1070;
        //        window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        //        window.Height = 630;

        //        var _page = App.Container.GetInstance<UserDashboardPageView>();
        //        var datacontext = App.Container.GetInstance<UserDasboardPageViewModel>();
        //        datacontext.User = user;
        //        _page.DataContext = datacontext;
        //        var currentPage = App.Container.GetInstance<UserHomePageView>();
        //        var data = App.Container.GetInstance<UserHomePageViewModel>();
        //        data.RefreshDataSource();
        //        data.User = user;
        //        currentPage.DataContext = data;
        //        datacontext.CurrentPage = currentPage;
        //        if (datacontext.PreviouslySelectedPanel != null)
        //            datacontext.PreviouslySelectedPanel.Tag = null;
        //        var stackPanel = _page.homePanel;
        //        stackPanel.Tag = "Selected";
        //        datacontext.PreviouslySelectedPanel = stackPanel;

        //        page.NavigationService.Navigate(_page);

        //    }
        //    else
        //        MessageBox.Show("Incorrect Password", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //else
        //    MessageBox.Show("Invalid Email", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

    }
    #endregion
}

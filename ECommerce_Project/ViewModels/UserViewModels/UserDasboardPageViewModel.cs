using ECommerce_Project.Command;
using ECommerce_Project.Entity.Models;
using ECommerce_Project.ViewModels.CommonViewModels;
using ECommerce_Project.Views.CommonViews;
using ECommerce_Project.Views.UserViews;
using Microsoft.EntityFrameworkCore;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace ECommerce_Project.ViewModels.UserViewModels;
public class UserDasboardPageViewModel : BaseViewModel
{
    private StackPanel previouslySelectedPanel;
    private Page currentPage;
    private User user;

    public User User { get => user; set { user = value; OnPropertyChanged(); } }
    public StackPanel PreviouslySelectedPanel { get => previouslySelectedPanel; set { previouslySelectedPanel = value; OnPropertyChanged(); } }
    public Page CurrentPage { get => currentPage; set { currentPage = value; OnPropertyChanged(); } }
    public UserDasboardPageViewModel()
    {
        PreviouslySelectedPanel = new StackPanel();
        HomeCommand = new RelayCommand(HomeCommandExecute);
        CartCommand = new RelayCommand(CartCommandExecute);
        MyOrderCommand = new RelayCommand(MyOrderCommandExecute);
        FavouritesCommand = new RelayCommand(FavouritesCommandExecute);
        ProfileCommand = new RelayCommand(ProfileCommandExecute);
        LogOutCommand = new RelayCommand(LogOutCommandExecute);
    }


    #region Commands


    #region HomeCommand

    public ICommand HomeCommand { get; set; }
    public void HomeCommandExecute(object? obj)
    {
        var stackPanel = obj as StackPanel;
        if (stackPanel == null) return;

        if (PreviouslySelectedPanel != null)
            PreviouslySelectedPanel.Tag = null;
        stackPanel.Tag = "Selected";
        PreviouslySelectedPanel = stackPanel;
        var _page = App.Container.GetInstance<UserHomePageView>();
        var datacontext = App.Container.GetInstance<UserHomePageViewModel>();
        datacontext.RefreshDataSource();
        datacontext.User = User;
        _page.DataContext = datacontext;

        CurrentPage = _page;
    }

    #endregion

    #region CartCommand

    public ICommand CartCommand { get; set; }
    public void CartCommandExecute(object? obj)
    {
        var stackPanel = obj as StackPanel;
        if (stackPanel == null) return;

        if (PreviouslySelectedPanel != null)
            PreviouslySelectedPanel.Tag = null;
        stackPanel.Tag = "Selected";
        PreviouslySelectedPanel = stackPanel;
        var _page = App.Container.GetInstance<UserShoppingCartPageView>();
        var datacontext = App.Container.GetInstance<UserShoppingCartPageViewModel>();
        //datacontext.RefreshDataSource();
        using var db = new AppDataContext();
        //var product = db.Products.Include(x => x.Category).FirstOrDefault();

        //var Prod = new ProductView();
        //Prod.Product = product;

        //var Prod = db.ProductViews.Include(x => x.Product).FirstOrDefault();
        //var Prod = db.ProductViews/*.Include(x => x.Product).ThenInclude(x => x.Category).*/.FirstOrDefault();
        //User.ShoppingCart.Add(Prod);
        datacontext.User = User;
        datacontext.RefreshDataSource();
        _page.DataContext = datacontext;
        CurrentPage = _page;
    }


    #endregion

    #region MyOrderCommand

    public ICommand MyOrderCommand { get; set; }
    public void MyOrderCommandExecute(object? obj)
    {
        var stackPanel = obj as StackPanel;
        if (stackPanel == null) return;

        if (PreviouslySelectedPanel != null)
            PreviouslySelectedPanel.Tag = null;
        stackPanel.Tag = "Selected";
        PreviouslySelectedPanel = stackPanel;
        var _page = App.Container.GetInstance<UserOrderPageView>();
        var datacontext = App.Container.GetInstance<UserOrderPageViewModel>();
        datacontext.User = User;
        datacontext.RefreshDataSource();
        _page.DataContext = datacontext;
        CurrentPage = _page;
    }


    #endregion

    #region FavouritesCommand

    public ICommand FavouritesCommand { get; set; }
    public void FavouritesCommandExecute(object? obj)
    {
        var stackPanel = obj as StackPanel;
        if (stackPanel == null) return;

        if (PreviouslySelectedPanel != null)
            PreviouslySelectedPanel.Tag = null;
        stackPanel.Tag = "Selected";
        PreviouslySelectedPanel = stackPanel;
        var _page = App.Container.GetInstance<UserFavouritesPageView>();
        var datacontext = App.Container.GetInstance<UserFavouritesPageViewModel>();
        datacontext.User = User;
        datacontext.RefreshDataSource();
        _page.DataContext = datacontext;
        CurrentPage = _page;
    }


    #endregion

    #region ProfileCommand

    public ICommand ProfileCommand { get; set; }
    public void ProfileCommandExecute(object? obj)
    {
        var stackPanel = obj as StackPanel;
        if (stackPanel == null) return;

        if (PreviouslySelectedPanel != null)
            PreviouslySelectedPanel.Tag = null;
        stackPanel.Tag = "Selected";
        PreviouslySelectedPanel = stackPanel;

        var _page = App.Container.GetInstance<UserProfilePageView>();
        var datacontext = App.Container.GetInstance<UserProfilePageViewModel>();
        datacontext.User1 = User;
        datacontext.RefreshDataSource();
        _page.DataContext = datacontext;
        CurrentPage = _page;
    }


    #endregion

    #region LogOutCommand
    public ICommand LogOutCommand { get; set; }
    public void LogOutCommandExecute(object? obj)
    {
        var page = obj as Page;
        var _page = App.Container.GetInstance<LoginPageView>();
        _page.DataContext = App.Container.GetInstance<LoginPageViewModel>();
        var window = NavigationWindow.GetWindow(page);
        window.Height = 450;
        window.Width = 800;
        window.ResizeMode = ResizeMode.NoResize;
        window.WindowStyle = WindowStyle.None;
        page.NavigationService.Navigate(_page);
    }

    #endregion


    #endregion

}

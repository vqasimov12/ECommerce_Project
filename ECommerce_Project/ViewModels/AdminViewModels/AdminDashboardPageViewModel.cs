using ECommerce_Project.Command;
using ECommerce_Project.ViewModels.CommonViewModels;
using ECommerce_Project.Views.AdminViews;
using ECommerce_Project.Views.CommonViews;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using System.Windows.Navigation;

namespace ECommerce_Project.ViewModels.AdminViewModels;

public class AdminDashboardPageViewModel : BaseViewModel
{
    private StackPanel previouslySelectedPanel;
    private Page currentPage;

    public StackPanel PreviouslySelectedPanel { get => previouslySelectedPanel; set { previouslySelectedPanel = value; OnPropertyChanged(); } }
    public Page CurrentPage { get => currentPage; set { currentPage = value; OnPropertyChanged(); } }

    public AdminDashboardPageViewModel()
    {
        CategoryCommand = new RelayCommand(CategoryCommandExecute);
        HomeCommand = new RelayCommand(HomeCommandExecute);
        CustomerCommand = new RelayCommand(CustomerCommandExecute);
        ProductCommand = new RelayCommand(ProductCommandExecute);
        OrderCommand = new RelayCommand(OrderCommandExecute);
        LogOutCommand = new RelayCommand(LogOutCommandExecute);
    }

    #region Commands

    #region CategoryCommand

    public ICommand CategoryCommand { get; set; }

    public void CategoryCommandExecute(object? obj)
    {
        var stackPanel = obj as StackPanel;
        if (stackPanel == null) return;

        if (PreviouslySelectedPanel != null)
            PreviouslySelectedPanel.Tag = null;
        stackPanel.Tag = "Selected";
        PreviouslySelectedPanel = stackPanel;
        var _page = App.Container.GetInstance<AdminCategoryPageView>();
        var datacontext = App.Container.GetInstance<AdminCategoryPageViewModel>();
        datacontext.RefreshCategories();
        _page.DataContext = datacontext;

        CurrentPage = _page;
    }
    #endregion

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
        var _page = App.Container.GetInstance<AdminHomePageView>();
        var datacontext = App.Container.GetInstance<AdminHomePageViewModel>();
        datacontext.RefreshDataSource();
        _page.DataContext = datacontext;
        CurrentPage = _page;
    }

    #endregion

    #region CustomerCommand
    public ICommand CustomerCommand { get; set; }

    public void CustomerCommandExecute(object? obj)
    {
        var stackPanel = obj as StackPanel;
        if (stackPanel == null) return;

        if (PreviouslySelectedPanel != null)
            PreviouslySelectedPanel.Tag = null;
        stackPanel.Tag = "Selected";
        PreviouslySelectedPanel = stackPanel;
        var _page = App.Container.GetInstance<AdminCustomersPageView>();
        var datacontext = App.Container.GetInstance<AdminCustomersPageViewModel>();
        datacontext.RefreshDataSource();
        _page.DataContext = datacontext;
        CurrentPage = _page;
    }

    #endregion

    #region ProductCommand
    public ICommand ProductCommand { get; set; }

    public void ProductCommandExecute(object? obj)
    {
        var stackPanel = obj as StackPanel;
        if (stackPanel == null) return;

        if (PreviouslySelectedPanel != null)
            PreviouslySelectedPanel.Tag = null;
        stackPanel.Tag = "Selected";
        PreviouslySelectedPanel = stackPanel;
        var _page = App.Container.GetInstance<AdminProductPageView>();
        var datacontext = App.Container.GetInstance<AdminProductPageViewModel>();
        datacontext.RefreshDataSource();
        _page.DataContext = datacontext;
        CurrentPage = _page;
    }

    #endregion

    #region OrderCommand
    public ICommand OrderCommand { get; set; }

    public void OrderCommandExecute(object? obj)
    {
        var stackPanel = obj as StackPanel;
        if (stackPanel == null) return;

        if (PreviouslySelectedPanel != null)
            PreviouslySelectedPanel.Tag = null;
        stackPanel.Tag = "Selected";
        PreviouslySelectedPanel = stackPanel;
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

using ECommerce_Project.Command;
using ECommerce_Project.Entity.Models;
using ECommerce_Project.ViewModels.AdminViewModels;
using ECommerce_Project.ViewModels.CommonViewModels;
using ECommerce_Project.Views.AdminViews;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
        //PreviouslySelectedPanel = stackPanel;
        //var _page = App.Container.GetInstance<AdminHomePageView>();
        //var datacontext = App.Container.GetInstance<AdminHomePageViewModel>();
        //datacontext.RefreshDataSource();
        //_page.DataContext = datacontext;
    }

    #endregion



    #endregion

}

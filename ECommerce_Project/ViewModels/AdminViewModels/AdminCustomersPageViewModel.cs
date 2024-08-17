using ECommerce_Project.Command;
using ECommerce_Project.Entity.Models;
using ECommerce_Project.ViewModels.CommonViewModels;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ECommerce_Project.ViewModels.AdminViewModels;
public class AdminCustomersPageViewModel : BaseViewModel
{
    private ObservableCollection<User> users;

    public ObservableCollection<User> Users { get => users; set { users = value; OnPropertyChanged(); } }
    public AdminCustomersPageViewModel()
    {
        RemoveCommand = new RelayCommand(RemoveCommandExecute);
        RefreshDataSource();
    }
    public void RefreshDataSource()
    {
        Users = new();
        using var db = new AppDataContext();
        foreach (var user in db.Users)
            Users.Add(user);
    }

    #region RemoveCommand
    public ICommand RemoveCommand { get; set; }
    public void RemoveCommandExecute(object? obj)
    {
        var user = obj as User;
        if (user is null) return;
        using var db = new AppDataContext();
        var _u = db.Users.FirstOrDefault(x => x.Id == user.Id);
        if (_u is null) return;
        db.Users.Remove(_u);
        db.SaveChanges();
        RefreshDataSource();
    }
    #endregion
}

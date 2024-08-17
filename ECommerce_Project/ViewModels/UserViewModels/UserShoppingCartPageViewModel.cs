using ECommerce_Project.Command;
using ECommerce_Project.Entity.Models;
using ECommerce_Project.ViewModels.CommonViewModels;
using Microsoft.EntityFrameworkCore;
using System.Windows;
using System.Windows.Input;

namespace ECommerce_Project.ViewModels.UserViewModels;
public class UserShoppingCartPageViewModel : BaseViewModel
{
    private User user;
    public int UserId { get; set; }
    public User User { get => user; set { user = value; OnPropertyChanged(); } }

    public UserShoppingCartPageViewModel()
    {
        IncreaseCommand = new RelayCommand(IncreaseCommandExecute, IncreaseCommandCanExecute);
        DecreaseCommand = new RelayCommand(DecreaseCommandExecute, DecreaseCommandCanExecute);
    }

    public void RefreshDataSource()
    {
        using var db = new AppDataContext();
        //User.ShoppingCart = new();
        //foreach(var i in db.ProductViews)
        //    if()
        //User.ShoppingCart
        User = db.Users
            .Include(x => x.ShoppingCart)
            .FirstOrDefault(x => x.Id == UserId)!;
        MessageBox.Show(User?.ShoppingCart.Count().ToString());


    }


    #region Commands

    #region IncreaseCommand
    public ICommand IncreaseCommand { get; set; }
    public bool IncreaseCommandCanExecute(object? obj)
    {
        var prod = obj as ProductView;
        if (prod is null) return false;
        return prod.Product?.Quantity >= prod.Count;

    }
    public void IncreaseCommandExecute(object? obj)
    {
        var prod = obj as ProductView;
        if (prod is null) return;
        prod.Count++;
    }

    #endregion

    #region DecreaseCommand
    public ICommand DecreaseCommand { get; set; }
    public bool DecreaseCommandCanExecute(object? obj)
    {
        var prod = obj as ProductView;
        if (prod is null) return false;
        return prod.Count > 1;

    }
    public void DecreaseCommandExecute(object? obj)
    {
        var prod = obj as ProductView;
        if (prod is null) return;
        prod.Count--;
    }

    #endregion

    #region RemoveCommand
    public ICommand RemoveCommand { get; set; }
    public void RemoveCommandExecute(object? obj)
    {
        var prod = obj as ProductView;
        if (prod is null) return;
        using var db = new AppDataContext();
        try
        {
            User.ShoppingCart.Remove(prod);
            db.SaveChanges();
        } 
        catch
        { }
    }
    #endregion

    #endregion

}

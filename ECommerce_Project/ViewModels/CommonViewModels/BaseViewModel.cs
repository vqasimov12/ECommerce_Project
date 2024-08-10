using ECommerce_Project.Command;
using System.Windows.Input;
using System.Windows;
using ECommerce_Project.Services;
using ECommerce_Project.Entity.Models;
using ECommerce_Project.Entity.Abstractions;

namespace ECommerce_Project.ViewModels.CommonViewModels;
public class BaseViewModel : NotifyPropertyChangedService
{
    public BaseViewModel()
    {
        CloseCommand = new RelayCommand(Close);
    }
    #region Functions

    #region CheckEmail
    public User CheckEmail(string email)
    {
        using var db = new AppDataContext();
        var a = (db.Users.FirstOrDefault(x => x.Email == email)) as User; ;
        return a;

    }

    #endregion

    #endregion


    #region Commands

    #region CloseCommand
    public ICommand CloseCommand { get; set; }
    public void Close(object? obj)
    {
        if (obj is FrameworkElement element)
        {
            Window window = Window.GetWindow(element);
            window?.Close();
        }
    }
    #endregion

    #endregion
}

using ECommerce_Project.Command;
using ECommerce_Project.Entity.Models;
using ECommerce_Project.ViewModels.CommonViewModels;
using Microsoft.EntityFrameworkCore;
using System.Windows.Forms;
using System.Windows.Input;

namespace ECommerce_Project.ViewModels.UserViewModels;
public class UserProductWindowViewModel : BaseViewModel
{
    int index = 0;
    public Product Product { get; set; }
    public UserProductWindowViewModel()
    {
        RightCommand = new RelayCommand(RightCommandExecute, RightCommandCanExecute);
        LeftCommand=new RelayCommand(LeftCommandExecute, LeftCommandCanExecute);
    }

    #region Commands

    #region RightCommand

    public ICommand RightCommand { get; set; }
    public bool RightCommandCanExecute(object?obj) => index < Product.Images.Count - 1;

    public void RightCommandExecute(object? obj)
    {
        if (index < Product.Images.Count - 1)
        {
            index++;
            Product.CurrentImage = Product.Images[index];
        }
    }



    #endregion

    #region LeftCommand

    public ICommand LeftCommand { get; set; }
    public bool LeftCommandCanExecute(object? obj) => index > 0;
    public void LeftCommandExecute(object? obj)
    {
        if (index > 0)
        {
            index--;
            Product.CurrentImage = Product.Images[index];
        }
    }



    #endregion

    #endregion
}

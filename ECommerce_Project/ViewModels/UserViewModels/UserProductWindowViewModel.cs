using ECommerce_Project.Command;
using ECommerce_Project.Entity.Models;
using ECommerce_Project.ViewModels.CommonViewModels;
using Microsoft.EntityFrameworkCore;
using System.Windows.Input;

namespace ECommerce_Project.ViewModels.UserViewModels;
public class UserProductWindowViewModel:BaseViewModel
{
    public Product Product { get; set; }
    //public ICommand CloseCommand { get; set; }
    //public UserProductWindowViewModel()
    //{
    //    CloseCommand = new RelayCommand(Close);
    //}
}

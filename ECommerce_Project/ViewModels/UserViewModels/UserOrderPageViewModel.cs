using ECommerce_Project.Command;
using ECommerce_Project.Entity.Models;
using ECommerce_Project.Services;
using ECommerce_Project.ViewModels.CommonViewModels;
using ECommerce_Project.Views.UserViews;
using Microsoft.EntityFrameworkCore;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace ECommerce_Project.ViewModels.UserViewModels;
public class UserOrderPageViewModel : BaseViewModel
{
    private User user;
    public User User { get => user; set { user = value; OnPropertyChanged(); } }

    public void RefreshDataSource()
    {
        using var db = new AppDataContext();
        User = db.Users.Include(z => z.Orders)
                       .ThenInclude(z => z.Products)
                       .ThenInclude(z => z.Product)
                       .ThenInclude(z => z.Category)
                       .FirstOrDefault(z => z.Id == User.Id)!;
    }

    public UserOrderPageViewModel()
    {
        FileDownloadCommand = new RelayCommand(FileDownloadCommandExecute);
        DetailsCommand=new RelayCommand(DetailsCommandExecute);
    }

    #region Commands

    #region FileDownloadCommand

    public ICommand FileDownloadCommand { get; set; }
    public void FileDownloadCommandExecute(object? obj)
    {
        var order = obj as Order;
        if (order is null) return;
        using var db = new AppDataContext();
        order = db.Orders.Include(z=>z.User).Include(z => z.Products).ThenInclude(z => z.Product).ThenInclude(z => z.Category).FirstOrDefault(z => z.Id == order.Id);
        var pdfsaver =new PdfSaverService();
        pdfsaver.ExportSelectedOrderToPdf(order);
    }

    #endregion

    #region DetailsCommand

    public ICommand DetailsCommand { get; set; }
    public void DetailsCommandExecute(object? obj)
    {
        var prodview = obj as ProductView;
        if (prodview is null) return;
        var prod=prodview.Product;
        var window = new UserProductWindowView();
        var datacontext = new UserProductWindowViewModel();
        using var db = new AppDataContext();
        prod = db.Products.Include(x => x.Category).FirstOrDefault(z => z.Id == prod.Id);
        if (prod is null) return;
        prod.CurrentImage = prod.CoverImage;
        datacontext.Product = prod;
        window.DataContext = datacontext;
        window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        window.WindowStyle = WindowStyle.None;

        window.ShowDialog();
    }

    #endregion
   
    #endregion

}

using ECommerce_Project.Command;
using ECommerce_Project.Entity.Models;
using ECommerce_Project.Services;
using ECommerce_Project.ViewModels.CommonViewModels;
using Microsoft.EntityFrameworkCore;
using System.Windows.Input;

namespace ECommerce_Project.ViewModels.AdminViewModels;
public class AdminOrderPageViewModel:BaseViewModel
{
    private List<Order> orders;

    public List<Order> Orders { get => orders; set{ orders = value; OnPropertyChanged(); }}

    public AdminOrderPageViewModel()
    {
        FileDownloadCommand = new RelayCommand(FileDownloadCommandExecute);
    }

    public void RefreshDataSource()
    {
        using var db = new AppDataContext();
        Orders=db.Orders.Include(z=>z.Products).ThenInclude(z=>z.Product).ThenInclude(z=>z.Category).Include(z=>z.User).ToList();
    }


    #region Commands

    #region FileDownloadCommand

    public ICommand FileDownloadCommand { get; set; }
    public void FileDownloadCommandExecute(object? obj)
    {
        var order = obj as Order;
        if (order is null) return;
        using var db = new AppDataContext();
        order = db.Orders.Include(z => z.User).Include(z => z.Products).ThenInclude(z => z.Product).ThenInclude(z => z.Category).FirstOrDefault(z => z.Id == order.Id);
        var pdfsaver = new PdfSaverService();
        pdfsaver.ExportSelectedOrderToPdf(order);
    }

    #endregion


    #endregion

}

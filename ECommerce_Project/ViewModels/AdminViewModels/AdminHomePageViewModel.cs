using ECommerce_Project.Command;
using ECommerce_Project.Entity.Models;
using ECommerce_Project.ViewModels.CommonViewModels;
using Microsoft.EntityFrameworkCore;
using System.Windows.Controls;
using System.Windows.Input;

namespace ECommerce_Project.ViewModels.AdminViewModels;
public class AdminHomePageViewModel : BaseViewModel
{
    private int? totalOrderCount;
    private int? totalCustomerCount;
    private double? totalIncome;
    private DateTime? _selectedDate;
    private Product topSellingProduct;
    private int topSelledCount;
    private string mostSoldCategory;
    private int totalProductsSold;
    private int topSoldCategoryCount;
    private List<Product> lowStockProducts;
    private bool listVisiblity;

    public int? TotalOrderCount { get => totalOrderCount; set { totalOrderCount = value; OnPropertyChanged(); } }
    public int? TotalCustomerCount { get => totalCustomerCount; set { totalCustomerCount = value; OnPropertyChanged(); } }

    public int TotalProductsSold { get => totalProductsSold; set { totalProductsSold = value; OnPropertyChanged(); } }
    public bool ListVisiblity { get => listVisiblity; set { listVisiblity = value; OnPropertyChanged(); }}
    public double? TotalIncome { get => totalIncome; set { totalIncome = value; OnPropertyChanged(); } }
    public DateTime? SelectedDate { get => _selectedDate; set { _selectedDate = value; OnPropertyChanged(); SearchForDate(); } }
    public Product TopSellingProduct { get => topSellingProduct; set { topSellingProduct = value; OnPropertyChanged(); } }
    public int TopSelledCount { get => topSelledCount; set { topSelledCount = value; OnPropertyChanged(); } }
    public List<Product> LowStockProducts { get => lowStockProducts; set { lowStockProducts = value; OnPropertyChanged(); } }
    public AdminHomePageViewModel()
    {
        RefreshCommand = new RelayCommand(RefreshCommandExecute);
    }

    void SearchForDate()
    {

        using var db = new AppDataContext();
        var orders = db.Orders.Where(x => x.OrderDate.Day == SelectedDate.Value.Day && x.OrderDate.Month == SelectedDate.Value.Month && x.OrderDate.Year == SelectedDate.Value.Year);

        TotalOrderCount = orders.Count();
        TotalIncome = orders.Sum(x => x.TotalPrice);
        TotalCustomerCount = db.Users.Count();
        TotalProductsSold = orders.SelectMany(order => order.Products)
    .Sum(pv => pv.Count ?? 0);
        var topSellingProduct = orders.SelectMany(order => order.Products)
      .GroupBy(pv => pv.Product)
      .Select(g => new
      {
          Product = g.Key,
          TotalSold = g.Sum(pv => pv.Count ?? 0)
      })
      .OrderByDescending(g => g.TotalSold)
      .FirstOrDefault();

        if (topSellingProduct != null)
        {
            TopSellingProduct = db.Products.Include(z => z.Category).FirstOrDefault(z => z.Id == topSellingProduct.Product.Id)!;
            TopSelledCount = topSellingProduct.TotalSold;
        }
    }

    public void RefreshDataSource()
    {
        using var db = new AppDataContext();
        TotalIncome = db.Orders.Sum(x => x.TotalPrice);
        TotalOrderCount = db.Orders.Count();
        totalCustomerCount = db.Users.Count();
        var orders = db.Orders.Include(z => z.Products).ThenInclude(z => z.Product).ThenInclude(z => z.Category);
        var topSellingProduct = orders
            .SelectMany(order => order.Products)
            .GroupBy(pv => pv.Product)
            .Select(g => new
            {
                Product = g.Key,
                TotalSold = g.Sum(pv => pv.Count ?? 0)
            })
            .OrderByDescending(g => g.TotalSold)
            .FirstOrDefault();

        if (topSellingProduct != null)
        {
            TopSellingProduct = db.Products.Include(z => z.Category).FirstOrDefault(z => z.Id == topSellingProduct.Product.Id)!;
            TopSelledCount = topSellingProduct.TotalSold;
        }
        TotalProductsSold = (int)orders.SelectMany(order => order.Products).Sum(x => x.Count);

        LowStockProducts = db.Products.Include(z => z.Category).Where(z => z.Quantity < 5).ToList();
        ListVisiblity = false;
        if (LowStockProducts.Count > 0)
            ListVisiblity = true;
    }

    #region Commands

    #region RefreshCommand

    public ICommand RefreshCommand { get; set; }
    public void RefreshCommandExecute(object? obj)
    {

        RefreshDataSource();
    }
    #endregion


    #endregion
}

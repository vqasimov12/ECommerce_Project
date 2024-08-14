using ECommerce_Project.Command;
using ECommerce_Project.Entity.Models;
using ECommerce_Project.ViewModels.CommonViewModels;
using System.Windows;
using System.Windows.Input;

namespace ECommerce_Project.ViewModels.AdminViewModels;
public class AdminHomePageViewModel : BaseViewModel
{
    private int? totalOrderCount;
    private int? totalCustomerCount;
    private double? totalIncome;
    private DateTime? _selectedDate;

    public int? TotalOrderCount { get => totalOrderCount; set { totalOrderCount = value; OnPropertyChanged(); } }
    public int? TotalCustomerCount { get => totalCustomerCount; set { totalCustomerCount = value; OnPropertyChanged(); } }
    public double? TotalIncome { get => totalIncome; set { totalIncome = value; OnPropertyChanged(); } }
    public DateTime? SelectedDate { get => _selectedDate; set { _selectedDate = value; OnPropertyChanged(); SearchForDate(); } }
    public AdminHomePageViewModel()
    {
        RefreshCommand = new RelayCommand(RefreshCommandExecute);
    }

    void SearchForDate()
    {

        using var db = new AppDataContext();
        TotalOrderCount = db.Orders.Where(x => x.OrderDate.Day == SelectedDate.Value.Day && x.OrderDate.Month == SelectedDate.Value.Month && x.OrderDate.Year == SelectedDate.Value.Year).Count();
        TotalIncome = db.Orders.Where(x => x.OrderDate.Day == SelectedDate.Value.Day && x.OrderDate.Month == SelectedDate.Value.Month && x.OrderDate.Year == SelectedDate.Value.Year).Sum(x => x.TotalPrice);
        TotalCustomerCount = db.Users.Count();
    }

    public void RefreshDataSource()
    {
        using var db = new AppDataContext();
        TotalIncome = db.Orders.Sum(x => x.TotalPrice);
        TotalOrderCount = db.Orders.Count();
        totalCustomerCount = db.Users.Count();
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

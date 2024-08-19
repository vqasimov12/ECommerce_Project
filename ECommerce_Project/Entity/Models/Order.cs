using ECommerce_Project.Services;
using System.Collections.ObjectModel;

namespace ECommerce_Project.Entity.Models;
public class Order:NotifyPropertyChangedService
{
    private ObservableCollection<ProductView> products = [];

    public int Id { get; set; }
    public int UserId { get; set; }
    public virtual User User { get; set; }
    public double? TotalPrice { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime DeliveryDate { get; set; }
    public ObservableCollection<ProductView> Products { get => products; set  { products = value; OnPropertyChanged(); } }
}

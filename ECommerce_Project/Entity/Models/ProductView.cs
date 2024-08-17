using ECommerce_Project.Services;
using System.Diagnostics;

namespace ECommerce_Project.Entity.Models;
public class ProductView : NotifyPropertyChangedService
{
    private Product product;
    private double? totalPrice;
    private int count;
    public ProductView()
    {
        Count = 1;
        TotalPrice = Product?.Price * Count;
    }
    public int Id { get; set; }
    public Product Product { get => product; set { product = value; OnPropertyChanged();TotalPrice = Product?.Price * Count; } }
    public double? TotalPrice { get => totalPrice; set { totalPrice = Product?.Price*Count; OnPropertyChanged(); } }
    public int Count { get => count; set { count = value; OnPropertyChanged(); TotalPrice = Product?.Price * Count; } }
}

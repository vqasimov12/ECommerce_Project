using System.Collections.ObjectModel;

namespace ECommerce_Project.Entity.Models;
public class Order
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public virtual User User { get; set; }
    public double TotalPrice { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime DeliveryDate { get; set; }
    public int Quantity { get; set; }
    public ObservableCollection<Product> Products { get; set; } = [];
}

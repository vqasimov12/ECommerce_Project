namespace ECommerce_Project.Entity.Models;
public class Product
{
    public int Id { get; set; }
    public string? ProductName {  get; set; }
    public string? ProductDescription { get; set;}
    public double? Price { get; set; }
    public int quantity {  get; set; }

    public Category Category { get; set; }
}
using ECommerce_Project.Services;

namespace ECommerce_Project.Entity.Models;
public class Product : NotifyPropertyChangedService
{
    private int id;
    private string? productName;
    private string? productDescription;
    private double? price;
    private int? quantity;
    private Category category;
    private int categoryId;
    private string image;

    public Product()
    {
        Image = @"https://res.cloudinary.com/doolsly8j/image/upload/v1723490972/ggfwvpxhtxevqpumkk5i.png";
    }
    public int Id { get => id; set { id = value; OnPropertyChanged(); } }
    public string? ProductName { get => productName; set { productName = value; OnPropertyChanged(); } }
    public string? ProductDescription { get => productDescription; set { productDescription = value; OnPropertyChanged(); } }
    public double? Price { get => price; set { price = value; OnPropertyChanged(); } }
    public int ?Quantity { get => quantity; set { quantity = value; OnPropertyChanged(); } }
    public int CategoryId { get => categoryId; set { categoryId = value; OnPropertyChanged(); } }
    public string Image { get => image; set { image = value; OnPropertyChanged(); } }
    public Category Category { get => category; set { category = value; OnPropertyChanged(); } }
    public void SetProduct(Product other)
    {
        ProductName = other.ProductName;
        ProductDescription = other.ProductDescription;
        Image=other.Image;
        Id = other.Id;
        CategoryId = other.CategoryId;
        Category = other.Category;
        Price = other.Price;
        Quantity = other.Quantity;
    }
}
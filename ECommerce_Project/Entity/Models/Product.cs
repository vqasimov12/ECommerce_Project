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
    private string? likeImage;
    private int rateCount;
    private double rating;
    private double ratingView;

    public Product()
    {
        Image = @"https://res.cloudinary.com/doolsly8j/image/upload/v1723490972/ggfwvpxhtxevqpumkk5i.png";
        RateCount = 0;
        LikeImage = @"/Icons/Dislike.png";
        Rating = 0;
        RatingView = 0;
    }

    public int Id { get => id; set { id = value; OnPropertyChanged(); } }
    public string? ProductDescription { get => productDescription; set { productDescription = value; OnPropertyChanged(); } }
    public int CategoryId { get => categoryId; set { categoryId = value; OnPropertyChanged(); } }
    public string Image { get => image; set { image = value; OnPropertyChanged(); } }
    public string? ProductName { get => productName; set { productName = value; OnPropertyChanged(); } }
    public double? Price { get => price; set { price = value; OnPropertyChanged(); } }
    public Category Category { get => category; set { category = value; OnPropertyChanged(); } }
    public int? Quantity { get => quantity; set { quantity = value; OnPropertyChanged(); } }
    public string? LikeImage { get => likeImage; set { likeImage = value; OnPropertyChanged(); } }
   
    //rating
    public double RatingView { get => ratingView; set { ratingView = value; OnPropertyChanged(); } }
    public double Rating { get => rating; set { rating = value; OnPropertyChanged(); RatingView = Rating / RateCount; } }
    public int RateCount { get => rateCount; set { rateCount = value; OnPropertyChanged(); RatingView = Rating / RateCount; } }
    
    public void SetProduct(Product other)
    {
        ProductName = other.ProductName;
        ProductDescription = other.ProductDescription;
        Image = other.Image;
        Id = other.Id;
        CategoryId = other.CategoryId;
        Category = other.Category;
        Price = other.Price;
        Quantity = other.Quantity;
        LikeImage = other.LikeImage;
    }
}
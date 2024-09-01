using ECommerce_Project.Services;
using System.Collections.ObjectModel;

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
    private string? likeImage;
    private int rateCount;
    private double rating;
    private double ratingView;
    private ObservableCollection<string> images=[];
    private string coverImage="";
    private string currentImage="";

    public Product()
    {
        CoverImage = @"https://res.cloudinary.com/doolsly8j/image/upload/v1724926263/nm2zhsf8uqfwdwjehkex.png";
        RateCount = 0;
        LikeImage = @"/Icons/Dislike.png";
        Rating = 0;
        RatingView = 0;
    }

    public int Id { get => id; set { id = value; OnPropertyChanged(); } }
    public string? ProductDescription { get => productDescription; set { productDescription = value; OnPropertyChanged(); } }
    public int CategoryId { get => categoryId; set { categoryId = value; OnPropertyChanged(); } }
    public string? ProductName { get => productName; set { productName = value; OnPropertyChanged(); } }
    public double? Price { get => price; set { price = value; OnPropertyChanged(); } }
    public Category Category { get => category; set { category = value; OnPropertyChanged(); } }
    public int? Quantity { get => quantity; set { quantity = value; OnPropertyChanged(); } }
    public string? LikeImage { get => likeImage; set { likeImage = value; OnPropertyChanged(); } }
    
    public ObservableCollection<string> Images { get => images; set  { images = value; OnPropertyChanged(); }}
    public string CoverImage { get => coverImage; set { coverImage = value; OnPropertyChanged(); }}
    public string CurrentImage { get => currentImage; set { currentImage = value; OnPropertyChanged(); }}

    //rating
    public double RatingView { get => ratingView; set { ratingView = value; OnPropertyChanged(); } }
    public double Rating { get => rating; set { rating = value; OnPropertyChanged(); RatingView = Rating / RateCount; } }
    public int RateCount { get => rateCount; set { rateCount = value; OnPropertyChanged(); RatingView = Rating / RateCount; } }
    
    public void SetProduct(Product other)
    {
        ProductName = other.ProductName;
        ProductDescription = other.ProductDescription;
        CurrentImage= other.CurrentImage;
        Id = other.Id;
        CategoryId = other.CategoryId;
        Category = other.Category;
        Price = other.Price;
        Quantity = other.Quantity;
        LikeImage = other.LikeImage;
        Images = other.Images;
        CurrentImage = other.CurrentImage;
    }
}
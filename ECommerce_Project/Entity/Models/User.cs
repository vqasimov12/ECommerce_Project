using ECommerce_Project.Entity.Abstractions;
using System.Collections.ObjectModel;

namespace ECommerce_Project.Entity.Models;
public class User : Person
{
    private ObservableCollection<Order> orders = [];
    private ObservableCollection<Product> favouriteProducts = [];
    private ObservableCollection<ProductView> shoppingCart = [];

    public ObservableCollection<Order> Orders { get => orders; set { orders = value; OnPropertyChanged(); } }
    public ObservableCollection<Product> FavouriteProducts { get => favouriteProducts; set { favouriteProducts = value; OnPropertyChanged(); } }
    public ObservableCollection<ProductView> ShoppingCart { get => shoppingCart; set { shoppingCart = value; OnPropertyChanged(); } }

    public User()
    {
        Image = @"https://res.cloudinary.com/doolsly8j/image/upload/v1723609855/i7th7hb5gytgbzprubqv.jpg";
    }

    public void SetUser(User _user)
    {
        Image = _user.Image;
        Name = _user.Name;
        Surname = _user.Surname;
        Email = _user.Email;
        Password = _user.Password;
        Address = _user.Address;
        Phone = _user.Phone;
        ShoppingCart = _user.ShoppingCart;
        FavouriteProducts = _user.FavouriteProducts;
        Orders = _user.Orders;
    }
    public static bool operator ==(User left, User right)
    {
        return (left?.Name == right?.Name && left?.Surname == right?.Surname && left?.Address == right?.Address && left?.Phone == right?.Phone && left?.Email == right?.Email && left?.Password == right?.Password);
    }
    public static bool operator !=(User left, User right)
    {
        return !(left == right);
    }
}

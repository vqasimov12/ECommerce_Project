using ECommerce_Project.Entity.Abstractions;
using System.Collections.ObjectModel;

namespace ECommerce_Project.Entity.Models;
public class User : Person
{
    private ObservableCollection<Product> orders;
    private ObservableCollection<Product> favouriteProducts;
    private ObservableCollection<Product> shoppingCart;

    public ObservableCollection<Product> Orders { get => orders; set { orders = value; OnPropertyChanged(); } }
    public ObservableCollection<Product> FavouriteProducts { get => favouriteProducts; set { favouriteProducts = value; OnPropertyChanged(); } }
    public ObservableCollection<Product> ShoppingCart { get => shoppingCart; set { shoppingCart = value; OnPropertyChanged(); } }

    public User()
    {

    }

    public void SetUser(User _user)
    {
        Name = _user.Name;
        Surname = _user.Surname;
        Email = _user.Email;
        Password=_user.Password;
        Address = _user.Address;
        Phone=_user.Phone;
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

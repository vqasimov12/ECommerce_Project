using ECommerce_Project.Entity.Models;
using ECommerce_Project.ViewModels.CommonViewModels;
using ECommerce_Project.Views.UserViews;
using Microsoft.EntityFrameworkCore;
using System.Windows.Input;
using System.Windows;
using ECommerce_Project.Command;

namespace ECommerce_Project.ViewModels.UserViewModels;
public class UserFavouritesPageViewModel : BaseViewModel
{
    private User user;
    private List<Product> products = [];

    public User User { get => user; set { user = value; OnPropertyChanged(); } }
    public List<Product> Products { get => products; set { products = value; OnPropertyChanged(); } }
    public UserFavouritesPageViewModel()
    {
        DetailsCommand = new RelayCommand(DetailsCommandExecute);
        LikeCommand = new RelayCommand(LikeCommandExecute);
        AddToCartCommand = new RelayCommand(AddToCartCommandExecute, AddToCartCommandCanExecute);
    }
    public void RefreshDataSource()
    {
        using var db = new AppDataContext();
        User = db.Users.Include(z=>z.ShoppingCart).Include(z => z.FavouriteProducts).ThenInclude(z => z.Category).FirstOrDefault(z => z.Id == User.Id)!;
        if (User is null) return;
        Products = User.FavouriteProducts.ToList();
    }



    #region Commands

    #region LikeCommand

    public ICommand LikeCommand { get; set; }
    //public bool LikeCommandCanExecite(object? obj)
    //{
    //    var prod = obj as Product;
    //    using var db = new AppDataContext();
    //    if (prod is not null)
    //    {
    //        User = db.Users.Include(x => x.FavouriteProducts).FirstOrDefault(x => x.Id == User.Id)!;
    //        if (User.FavouriteProducts.Any(x => x.Id == prod.Id))
    //            prod.LikeImage = "../../Icons/Like.png";
    //        else
    //            prod.LikeImage = "../../Icons/DisLike.png";
    //    }
    //    return true;
    //}
    public void LikeCommandExecute(object? obj)
    {
        var prod = obj as Product;
        using var db = new AppDataContext();
        if (prod is null) return;
        User = db.Users.Include(x => x.FavouriteProducts).FirstOrDefault(x => x.Id == User.Id)!;
        if (User.FavouriteProducts.Any(x => x.Id == prod.Id))
        {
            prod.LikeImage = "../../Icons/Dislike.png";
            foreach (var i in User.FavouriteProducts)
                if (i.Id == prod.Id)
                {
                    user.FavouriteProducts.Remove(i);
                    break;
                }
        }
        else
        {
            prod.LikeImage = "../../Icons/Like.png";
            User.FavouriteProducts.Add(prod);
        }
        db.SaveChanges();
    }

    #endregion

    #region AddToCartCommand

    public bool AddToCartCommandCanExecute(object? obj)
    {
        var prod = obj as Product;
        if(prod is null) return false;
        using var db = new AppDataContext();
        User = db.Users.Include(x => x.ShoppingCart).Include(z => z.FavouriteProducts).ThenInclude(z => z.Category).FirstOrDefault(x => x.Id == User.Id)!;
        prod = db.Products.Include(x => x.Category).FirstOrDefault(x => x.Id == prod.Id);
        if (prod?.Quantity < 1) return false;

        if (!User.ShoppingCart.Any(z => z.Product.Id == prod?.Id))
            return true;
        return false;
    }
    public ICommand AddToCartCommand { get; set; }
    public void AddToCartCommandExecute(object? obj)
    {
        var prod = obj as Product;
        if (prod is null) return;
        using var db = new AppDataContext();
        User = db.Users.Include(x => x.ShoppingCart).Include(z => z.FavouriteProducts).ThenInclude(z => z.Category).FirstOrDefault(x => x.Id == User.Id)!;
        prod = db.Products.Include(x => x.Category).FirstOrDefault(x => x.Id == prod.Id);
        if (prod is null) return;
        ProductView a = new ProductView() { Count = 1, Product = prod };
        User.ShoppingCart.Add(a);
        db.SaveChanges();
    }


    #endregion

    #region DetailsCommand

    public ICommand DetailsCommand { get; set; }
    public void DetailsCommandExecute(object? obj)
    {
        var prod = obj as Product;
        if (prod is null) return;
        var window = new UserProductWindowView();
        var datacontext = new UserProductWindowViewModel();
        using var db = new AppDataContext();
        prod = db.Products.Include(x => x.Category).FirstOrDefault(z => z.Id == prod.Id);
        if (prod is null) return;
        prod.CurrentImage = prod.CoverImage;
        datacontext.Product = prod;
        window.DataContext = datacontext;
        window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        window.WindowStyle = WindowStyle.None;
     
        window.ShowDialog();
    }

    #endregion

    #endregion

}

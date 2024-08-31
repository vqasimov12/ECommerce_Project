using ECommerce_Project.Command;
using ECommerce_Project.Entity.Models;
using ECommerce_Project.ViewModels.CommonViewModels;
using ECommerce_Project.Views.UserViews;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ECommerce_Project.ViewModels.UserViewModels;
public class UserHomePageViewModel : BaseViewModel
{
    private ObservableCollection<Product> products = [];
    private ObservableCollection<Category> categories = [];
    private Category? categoryFilter;
    private string? nameFilter;
    private string? descriptionFilter;
    private double? minPrice;
    private double? maxPrice;
    private User user;
    private double ratingFilter;

    public User User { get => user; set { user = value; OnPropertyChanged(); } }
    public double? MaxPrice { get => maxPrice; set { maxPrice = value; OnPropertyChanged(); } }
    public double? MinPrice { get => minPrice; set { minPrice = value; OnPropertyChanged(); } }
    public double RatingFilter { get => ratingFilter; set { ratingFilter = value; OnPropertyChanged(); } }
    public string? DescriptionFilter { get => descriptionFilter; set { descriptionFilter = value; OnPropertyChanged(); } }
    public string? NameFilter { get => nameFilter; set { nameFilter = value; OnPropertyChanged(); } }
    public Category? CategoryFilter { get => categoryFilter; set { categoryFilter = value; OnPropertyChanged(); } }

    public ObservableCollection<Product> Products { get => products; set { products = value; OnPropertyChanged(); } }
    public ObservableCollection<Category> Categories { get => categories; set { categories = value; OnPropertyChanged(); } }

    public UserHomePageViewModel()
    {
        RefreshDataSource();
        RefreshCommand = new RelayCommand(RefreshCommandExecute);
        ApplyFiltersCommand = new RelayCommand(ApplyFiltersCommandExecute);
        LikeCommand = new RelayCommand(LikeCommandExecute, LikeCommandCanExecite);
        AddToCartCommand = new RelayCommand(AddToCartCommandExecute, AddToCartCommandCanExecute);
        DetailsCommand = new RelayCommand(DetailsCommandExecute);
    }

    public void RefreshDataSource()
    {
        Products = new();
        Categories = new();
        var db = new AppDataContext();
        foreach (var i in db.Products)
            Products.Add(i);
        foreach (var i in db.Categories)
            Categories.Add(i);
        MaxPrice = null;
        MinPrice = null;
        DescriptionFilter = "";
        NameFilter = "";
        RatingFilter = 0;
    }

    #region Commands

    #region RefreshCommand

    public ICommand RefreshCommand { get; set; }
    public void RefreshCommandExecute(object? obj)
    {
        RefreshDataSource();
    }

    #endregion

    #region ApplyFiltersCommand

    public ICommand ApplyFiltersCommand { get; set; }

    public void ApplyFiltersCommandExecute(object? obj)
    {
        if (MaxPrice.HasValue && MinPrice.HasValue && MaxPrice < MinPrice)
        {
            System.Windows.Forms.MessageBox.Show("Max Price can not be higher than Min Price");
            MaxPrice = null;
        }
        using var db = new AppDataContext();
        List<Product> tempProducts = db.Products.Include(z=>z.Category).ToList();
        if (CategoryFilter is not null)
        {
            var combobox = obj as ComboBox;
            if (combobox is not null && combobox.SelectedItem is not null)
            {
                var cat = combobox.SelectedItem as Category;
                if (cat is not null)
                    tempProducts = db.Products.Include(x => x.Category).Where(x => x.Category.Name == cat.Name).ToList();
            }
        }
        if (minPrice is not null)
            tempProducts = tempProducts.Where(x => x.Price >= MinPrice).ToList();
        if (maxPrice is not null)
            tempProducts = tempProducts.Where(x => x.Price <= MaxPrice).ToList();
        if (NameFilter != "")
            tempProducts = tempProducts.Where(x => x.ProductName.Contains(NameFilter)).ToList();
        if (descriptionFilter != "")
            tempProducts = tempProducts.Where(x => x.ProductDescription.Contains(DescriptionFilter)).ToList();
        tempProducts = tempProducts.Where(x => x.RatingView >= RatingFilter).ToList();
        Products = new(tempProducts);
    }

    #endregion

    #region LikeCommand

    public ICommand LikeCommand { get; set; }
    public bool LikeCommandCanExecite(object? obj)
    {
        var prod = obj as Product;
        using var db = new AppDataContext();
        if (prod is not null)
        {
            User = db.Users.Include(x => x.FavouriteProducts).FirstOrDefault(x => x.Id == User.Id)!;
            if (User.FavouriteProducts.Any(x => x.Id == prod.Id))
                prod.LikeImage = "../../Icons/Like.png";
            else
                prod.LikeImage = "../../Icons/DisLike.png";
        }
        return true;
    }
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

    public ICommand AddToCartCommand { get; set; }
    public bool AddToCartCommandCanExecute(object? obj)
    {
        var prod = obj as Product;
        if (prod is null) return false;
        using var db = new AppDataContext();
        User = db.Users.Include(x => x.ShoppingCart).Include(z => z.FavouriteProducts).ThenInclude(z => z.Category).FirstOrDefault(x => x.Id == User.Id)!;
        prod = db.Products.Include(x => x.Category).FirstOrDefault(x => x.Id == prod.Id);
        if (prod.Quantity < 1) return false;
        if (!User.ShoppingCart.Any(z => z.Product.Id == prod?.Id))
            return true;
        return false;
    }
    public void AddToCartCommandExecute(object? obj)
    {
        var prod = obj as Product;
        if (prod is null) return;
        using var db = new AppDataContext();
        User = db.Users.Include(x => x.ShoppingCart).FirstOrDefault(x => x.Id == User.Id)!;
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
        datacontext.Product = prod;
        window.DataContext = datacontext;
        window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        window.WindowStyle = WindowStyle.None;
        window.ShowDialog();
    }

    #endregion

    #endregion

}

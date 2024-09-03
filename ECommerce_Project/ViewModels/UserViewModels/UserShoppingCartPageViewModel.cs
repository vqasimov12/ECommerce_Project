using ECommerce_Project.Command;
using ECommerce_Project.Entity.Models;
using ECommerce_Project.Services;
using ECommerce_Project.ViewModels.CommonViewModels;
using ECommerce_Project.Views.UserViews;
using Microsoft.EntityFrameworkCore;
using System.Windows;
using System.Windows.Input;

namespace ECommerce_Project.ViewModels.UserViewModels;
public class UserShoppingCartPageViewModel : BaseViewModel
{
    private User user;
    private List<ProductView> products = [];
    private double? subTotal;

    public bool CanPurchase = false;
    public double? SubTotal { get => subTotal; set { subTotal = value; OnPropertyChanged(); } }
    public User User { get => user; set { user = value; OnPropertyChanged(); } }
    public List<ProductView> Products { get => products; set { products = value; OnPropertyChanged(); } }
    public UserShoppingCartPageViewModel()
    {
        RemoveCommand = new RelayCommand(RemoveCommandExecute);
        IncreaseCommand = new RelayCommand(IncreaseCommandExecute, IncreaseCommandCanExecute);
        DecreaseCommand = new RelayCommand(DecreaseCommandExecute, DecreaseCommandCanExecute);
        PurchaseCommand = new RelayCommand(PurchaseCommandExecute, PurchaseCommandCanExecute);
        DetailsCommand = new RelayCommand(DetailsCommandExecute);
    }

    public void RefreshDataSource()
    {
        using var db = new AppDataContext();
        User = db.Users
            .Include(x => x.ShoppingCart)
            .ThenInclude(sc => sc.Product)
            .ThenInclude(p => p.Category)
            .FirstOrDefault(x => x.Id == User.Id)!;
        Products = User.ShoppingCart.ToList();
        SubTotal = Products.Sum(p => p.TotalPrice ?? 0);
        CanPurchase = false;
    }

    #region Commands

    #region IncreaseCommand
    public ICommand IncreaseCommand { get; set; }
    public bool IncreaseCommandCanExecute(object? obj)
    {
        using var db = new AppDataContext();
        User = db.Users
            .Include(x => x.ShoppingCart).ThenInclude(z => z.Product)
            .FirstOrDefault(x => x.Id == User.Id)!;
        var prod = obj as ProductView;
        if (prod is null) return false;
        var cartItem = User.ShoppingCart
       .FirstOrDefault(z => z.Product.Id == prod.Product.Id);
        if (cartItem is null) return false;
        return cartItem.Count < cartItem.Product.Quantity;
    }
    public void IncreaseCommandExecute(object? obj)
    {
        using var db = new AppDataContext();
        User = db.Users
            .Include(x => x.ShoppingCart).ThenInclude(z => z.Product)
            .FirstOrDefault(x => x.Id == User.Id)!;

        var prod = obj as ProductView;
        if (prod is null) return;
        var cartItem = User.ShoppingCart
       .FirstOrDefault(z => z.Product.Id == prod.Product.Id);
        if (cartItem is null) return;
        cartItem.Count++;
        db.SaveChanges();
        RefreshDataSource();

    }

    #endregion

    #region DecreaseCommand
    public ICommand DecreaseCommand { get; set; }
    public bool DecreaseCommandCanExecute(object? obj)
    {
        var prod = obj as ProductView;
        if (prod is null) return false;
        return prod.Count > 1;

    }
    public void DecreaseCommandExecute(object? obj)
    {
        using var db = new AppDataContext();
        User = db.Users
            .Include(x => x.ShoppingCart).ThenInclude(z => z.Product)
            .FirstOrDefault(x => x.Id == User.Id)!;

        var prod = obj as ProductView;
        if (prod is null) return;
        var cartItem = User.ShoppingCart
       .FirstOrDefault(z => z.Product.Id == prod.Product.Id);
        if (cartItem is null) return;
        cartItem.Count--;
        db.SaveChanges();
        RefreshDataSource();
    }

    #endregion

    #region RemoveCommand
    public ICommand RemoveCommand { get; set; }
    public void RemoveCommandExecute(object? obj)
    {
        if (MessageBox.Show("Are you sure to delete this product from cart?", "Removing", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            try
            {
                using var db = new AppDataContext();
                User = db.Users
                    .Include(x => x.ShoppingCart).ThenInclude(z => z.Product)
                    .FirstOrDefault(x => x.Id == User.Id)!;

                var prod = obj as ProductView;
                if (prod is null) return;
                var cartItem = User.ShoppingCart
               .Where(z => z.Product.Id == prod.Product.Id).ToList();
                if (cartItem.Count == 0) return;
                foreach (var item in cartItem)
                    User.ShoppingCart.Remove(item);
                db.SaveChanges();
                RefreshDataSource();
            }
            catch
            { }
    }
    #endregion

    #region PurchaseCommand
    public ICommand PurchaseCommand { get; set; }
    public bool PurchaseCommandCanExecute(object? obj)
    {
        if (SubTotal <= 0) return false;
        foreach (var cartItem in User.ShoppingCart)
            if (cartItem.Count > cartItem.Product.Quantity)
                return false;
        return true;
    }
    public void PurchaseCommandExecute(object? obj)
    {
        if (CanPurchase)
        {
            using var db = new AppDataContext();
            User = db.Users.Include(z => z.Orders).Include(z => z.ShoppingCart).ThenInclude(z => z.Product).ThenInclude(z => z.Category).FirstOrDefault(z => z.Id == User.Id)!;

            var order = new Order { OrderDate = DateTime.Now, Products = User.ShoppingCart, TotalPrice = SubTotal, User = User, DeliveryDate = DateTime.Now.AddDays(3) };
            User.Orders.Add(order);
            foreach (var cartItem in User.ShoppingCart)
            {
                var product = db.Products.FirstOrDefault(p => p.Id == cartItem.Product.Id);
                if (product != null && product.Quantity >= cartItem.Count)
                    product.Quantity -= cartItem.Count;
            }

            {
                var window = new RateProductWindowView();
                var context = new RateProductWindowViewModel();
                context.Products = User.ShoppingCart;
                window.DataContext = context;

                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                window.Show();

                MailService.SendMail(User.Email!, User.ShoppingCart);
            }

            User.ShoppingCart = new();
            db.SaveChanges();
            RefreshDataSource();
        }
        else
        {
            var model = App.Container.GetInstance<UserDasboardPageViewModel>();
            var datacontext = App.Container.GetInstance<UserPaymentPageViewModel>();
            var page = App.Container.GetInstance<UserPaymentPageView>();
            datacontext.RefreshDataSource();
            page.DataContext = datacontext;
            model.CurrentPage = page;
        }
    }

    #endregion

    #region DetailsCommand

    public ICommand DetailsCommand { get; set; }
    public void DetailsCommandExecute(object? obj)
    {
        var prodview = obj as ProductView;
        if (prodview is null) return;
        var prod = prodview.Product;
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

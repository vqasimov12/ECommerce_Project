using ECommerce_Project.Command;
using ECommerce_Project.Entity.Models;
using ECommerce_Project.ViewModels.CommonViewModels;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using ECommerce_Project.Views.AdminViews;
using System.Text.RegularExpressions;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;

namespace ECommerce_Project.ViewModels.AdminViewModels;
public class AdminProductPageViewModel : BaseViewModel
{
    private Product product;
    private ObservableCollection<Category> categories;
    private ObservableCollection<Product> products;
    private bool _addVisible = true;
    private Category category;
    private bool isEditting = false;
    private string productPrice;

    public Product Product { get => product; set { product = value; OnPropertyChanged(); } }
    public Category Category { get => category; set { category = value; Product.Category = value; OnPropertyChanged(); } }
    public ObservableCollection<Category> Categories { get => categories; set { categories = value; OnPropertyChanged(); } }
    public ObservableCollection<Product> Products { get => products; set { products = value; OnPropertyChanged(); } }
    public string ProductPrice
    {
        get => productPrice; set
        {
            if (Regex.IsMatch(value, @"^[0-9]{0,6}(?:\.[0-9]{0,2})?$") || value == "")
            {
                productPrice = value; double i;
                if (double.TryParse(value, out i))
                    Product.Price = i;
                OnPropertyChanged();
            }
        }
    }
    public AdminProductPageViewModel()
    {
        Product = new();
        AddCommand = new RelayCommand(AddCommandExecute, AddCommandCanExecute);
        EditCommand = new RelayCommand(EditCommandExecute, EditCommandCanExecute);
        DeleteCommand = new RelayCommand(DeleteCommandExecute, DeleteCommandCanExecute);
        SelectImageCommand = new RelayCommand(SelectImageCommandExecute);
        SearchCommand = new RelayCommand(SearchCommandExecute);
        RefreshCommand = new RelayCommand(RefreshCommandExecute);
        RefreshDataSource();
    }
    public void RefreshDataSource()
    {
        Categories = new();
        Products = new();
        Product = new();
        _addVisible = true;
        Category = new();
        ProductPrice = "";
        isEditting = false;
        using var db = new AppDataContext();
        var ordercat = db.Categories.OrderBy(x => x.Id);
        var orderprod = db.Products.OrderBy(x => x.Id);
        foreach (var i in ordercat)
            Categories.Add(i);
        foreach (var i in orderprod)
            Products.Add(i);
    }

    #region Commands

    #region AddCommand

    public ICommand AddCommand { get; set; }
    public bool AddCommandCanExecute(object? obj)
    {
        if (isEditting) return false;
        if (_addVisible && Product?.ProductName?.Length >= 3 && Product?.ProductDescription?.Length >= 2 && Product?.Price > 0&&!string.IsNullOrEmpty(Product?.Category?.Name) )
            return true;
        return false;
    }
    public void AddCommandExecute(object? obj)
    {
        using var db = new AppDataContext();
        var cat = db.Categories.FirstOrDefault(x => x.Name == Category.Name);
        Product.Category = cat;
        Product.Quantity ??= 0;
        db.Products.Add(Product);
        db.SaveChanges();
        RefreshDataSource();
        Product = new();
    }

    #endregion

    #region EditCommand
    public ICommand EditCommand { get; set; }
    public bool EditCommandCanExecute(object? obj)
    {
        if (isEditting)
            return Product.Price > 0 && Product.ProductName?.Length > 2 && Product.ProductDescription?.Length >= 3 && !string.IsNullOrEmpty(Product?.Category?.Name);
        var lw = obj as ListView;
        return lw?.SelectedItem is not null;
    }

    public void EditCommandExecute(object? obj)
    {
        if (!isEditting)
        {
            _addVisible = false;
            var lw = obj as ListView;
            if (lw is null) return;
            var _prod = lw.SelectedItem as Product;
            if (_prod is null) return;
            Product.SetProduct(_prod);
            Category = Product.Category;
            ProductPrice = _prod.Price.ToString();
            isEditting = true;
        }
        else
        {
            isEditting = false;
            _addVisible = true;
            using var db = new AppDataContext();
            var pr = db.Products.FirstOrDefault(x => x.Id == Product.Id);
            Product.Category = Category;
            pr.SetProduct(Product);
            db.SaveChanges();
            Product = new();
            Category = new();
            RefreshDataSource();
        }
    }
    #endregion

    #region DeleteCommand

    public ICommand DeleteCommand { get; set; }

    public bool DeleteCommandCanExecute(object? obj)
    {
        var lw = obj as ListView;

        if (lw is null)
            return false;
        return lw.SelectedItem is not null;

    }
    public void DeleteCommandExecute(object? obj)
    {
        using var db = new AppDataContext();
        var _lw = obj as ListView;
        var _pr = _lw?.SelectedItem as Product;
        if (_pr is null) return;

        var pr = db.Products.FirstOrDefault(x => x.Id == _pr.Id);
        db.Products.Remove(pr);
        db.SaveChanges();
        RefreshDataSource();
    }

    #endregion

    #region SelectImageCommand

    public ICommand SelectImageCommand { get; set; }
    public void SelectImageCommandExecute(object? obj)
    {
        var window = new AdminEditProductImagesWindowView();
        var data = new AdminEditProductImagesWindowViewModel();
        data.Refresh();
        data.ProductImages = Product.Images;
        data.CoverImage = Product.CoverImage;
        window.DataContext = data;
        window.ShowDialog();
        //Product.Image = result;
    }

    #endregion

    #region SearchCommand
    public ICommand SearchCommand { get; set; }
    public void SearchCommandExecute(object? obj)
    {
        var cb = obj as ComboBox;
        if (cb is null || cb.SelectedItem is null) return;
        var cat = cb.SelectedItem as Category;
        if (cat is null) return;
        using var db = new AppDataContext();
        var _p = db.Products
               .Include(x => x.Category)
               .Where(x => x.Category.Name == cat.Name)
               .ToList();
        Products = new ObservableCollection<Product>(_p);
    }
    #endregion

    #region RefreshCommand  
    public ICommand RefreshCommand { get; set; }
    public void RefreshCommandExecute(object? obj)
    {
        RefreshDataSource();
    }

    #endregion

    #endregion

}

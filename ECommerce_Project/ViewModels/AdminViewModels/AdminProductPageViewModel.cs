using ECommerce_Project.Command;
using ECommerce_Project.Entity.Models;
using ECommerce_Project.ViewModels.CommonViewModels;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.EntityFrameworkCore;

namespace ECommerce_Project.ViewModels.AdminViewModels;
public class AdminProductPageViewModel : BaseViewModel
{
    private Product product;
    private ObservableCollection<Category> categories;
    private ObservableCollection<Product> products;
    private bool _addVisible = true;
    private Category category;
    private bool isEditting = false;
    private Account account = new Account(
      "doolsly8j",
      "445179498452818",
      "SMlohF-hU9X8GBILv3FqTX_Q2Ok");
    private Cloudinary cloudinary { get; set; }

    public Product Product { get => product; set { product = value; OnPropertyChanged(); } }
    public Category Category { get => category; set { category = value; OnPropertyChanged(); } }
    public ObservableCollection<Category> Categories { get => categories; set { categories = value; OnPropertyChanged(); } }
    public ObservableCollection<Product> Products { get => products; set { products = value; OnPropertyChanged(); } }

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
        cloudinary = new Cloudinary(account);
    }
    public void RefreshDataSource()
    {
        Categories = new();
        Products = new();
        Product = new();
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
        if (_addVisible && Product.ProductName?.Length >= 3 && Product.ProductDescription?.Length >= 2 && Product.Price > 0 && Category is not null)
            return true;
        return false;
    }
    public void AddCommandExecute(object? obj)
    {
        using var db = new AppDataContext();
        var cat = db.Categories.FirstOrDefault(x => x.Name == Category.Name);
        Product.Category = cat;

        var uploadParams = new ImageUploadParams()
        {
            File = new FileDescription(Product.Image)
        };

        var uploadResult = cloudinary.Upload(uploadParams);
        string imageUrl = uploadResult.SecureUrl.ToString();
        Product.Quantity ??= 0;
        Product.Image = imageUrl;
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
            return Product.Price > 0 && Product.ProductName?.Length > 2 && Product.ProductDescription?.Length >= 3 && Product.Category.Name is not null;
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
            isEditting = true;
        }
        else
        {
            isEditting = false;
            _addVisible = true;
            using var db = new AppDataContext();
            var pr = db.Products.FirstOrDefault(x => x.Id == Product.Id);
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(Product.Image)
            };

            var uploadResult = cloudinary.Upload(uploadParams);
            string imageUrl = uploadResult.SecureUrl.ToString();
            Product.Quantity ??= 0;
            Product.Image = imageUrl;
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
        System.Windows.Forms.OpenFileDialog dlg = new System.Windows.Forms.OpenFileDialog();
        dlg.Filter = "Image files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg";
        dlg.ShowDialog();
        string? result = dlg.FileName;
        if (result is null)
            return;
        Product.Image = result;
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

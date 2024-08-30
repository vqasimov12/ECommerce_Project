using ECommerce_Project.Command;
using ECommerce_Project.Entity.Models;
using ECommerce_Project.ViewModels.CommonViewModels;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ECommerce_Project.ViewModels.UserViewModels;
public class RateProductWindowViewModel : BaseViewModel
{
    private ObservableCollection<ProductView> products;

    public ObservableCollection<ProductView> Products { get => products; set { products = value; OnPropertyChanged(); } }
    //public User User { get; set; }
    public RateProductWindowViewModel()
    {
        CommitCommand = new RelayCommand(CommitCommandExecute);
    }
    public ICommand CommitCommand { get; set; }
    public void CommitCommandExecute(object? obj)
    {
        var db = new AppDataContext();
        var temp = db.Products.ToList();
        foreach (var i in Products)
            foreach (var t in temp)
                if (i.Product.Id == t.Id)
                {
                    var r = t.RatingView * t.RateCount;
                    r += i.Product.Rating;
                    t.RateCount++;
                    t.RatingView = r / t.RateCount;
                    i.Product.Rating = 0;
                    break;
                }
        db.SaveChanges();
        Close(obj);
    }


}


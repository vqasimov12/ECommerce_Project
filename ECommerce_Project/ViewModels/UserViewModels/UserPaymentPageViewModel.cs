using ECommerce_Project.Command;
using ECommerce_Project.ViewModels.CommonViewModels;
using ECommerce_Project.Views.UserViews;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace ECommerce_Project.ViewModels.UserViewModels;

public class UserPaymentPageViewModel : BaseViewModel
{
    private bool secondPanelVisiblity = false;
    private bool firstPanelVisiblity = true;
    private bool deliveryChecked;
    private bool cardChecked;
    private bool paypalCkecked;
    private string cvvCode = "";
    private string cardNumber;
    private string expDate;

    public bool SecondPanelVisiblity { get => secondPanelVisiblity; set { secondPanelVisiblity = value; OnPropertyChanged(); } }
    public bool FirstPanelVisiblity { get => firstPanelVisiblity; set { firstPanelVisiblity = value; OnPropertyChanged(); } }

    public bool DeliveryChecked { get => deliveryChecked; set { deliveryChecked = value; OnPropertyChanged(); SecondPanelVisiblity = false; FirstPanelVisiblity = true; } }
    public bool CardChecked { get => cardChecked; set { cardChecked = value; OnPropertyChanged(); SecondPanelVisiblity = true; FirstPanelVisiblity = false; } }
    public bool PaypalCkecked { get => paypalCkecked; set { paypalCkecked = value; OnPropertyChanged(); SecondPanelVisiblity = false; FirstPanelVisiblity = true; } }
    public string CardNumber
    {
        get => cardNumber;
        set
        {
            if (Regex.IsMatch(value, @"^[0-9]{0,16}$"))
            {
                cardNumber = value;
                OnPropertyChanged();
            }
        }
    }
    public string CvvCode
    {
        get => cvvCode;
        set
        {
            int a;
            if (value.Length == 0)
                cvvCode = "";
            if (int.TryParse(value, out a) && a < 1000)
            {
                cvvCode = value;
                OnPropertyChanged();
            }
        }
    }
    public string ExpDate
    {
        get => expDate;
        set
        {
            var a = value;

            if (a.Contains("/"))
                a = a.Replace("/", "");
            if (!Regex.IsMatch(a, "^[0-9]{0,4}$"))
                return;
            if (a.Length >= 2)
                a = a.Insert(2, "/");
            if (a.Length <= 5)
            {
                expDate = a;
                OnPropertyChanged();
            }
        }
    }

    public UserPaymentPageViewModel()
    {
        BackCommand = new RelayCommand(BackCommandExecute);
        ConfirmCommand = new RelayCommand(ConfirmCommandExecute);
    }
    public void RefreshDataSource()
    {
        DeliveryChecked = true;
        SecondPanelVisiblity = false;
        FirstPanelVisiblity = true;
        CardNumber = "";
        CvvCode = "";
        ExpDate = "";
    }


    #region Commands

    #region BackCommand

    public ICommand BackCommand { get; set; }
    public void BackCommandExecute(object? obj)
    {
        var model = App.Container.GetInstance<UserDasboardPageViewModel>();
        var datacontext = App.Container.GetInstance<UserShoppingCartPageViewModel>();
        var page = App.Container.GetInstance<UserShoppingCartPageView>();
        datacontext.RefreshDataSource();
        page.DataContext = datacontext;
        model.CurrentPage = page;
    }

    #endregion

    #region ConfirmCommand

    public ICommand ConfirmCommand { get; set; }
    public void ConfirmCommandExecute(object? obj)
    {
        var model = App.Container.GetInstance<UserShoppingCartPageViewModel>();
        model.CanPurchase = false;
        if (CardChecked)
        {
            if (ExpDate.Length != 5)
            {
                MessageBox.Show("Invalid Date", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }
            var a = ExpDate.Substring(0, 2);
            int y, m;
            int currentY = DateTime.Now.Year % 100;
            int currentM = DateTime.Now.Month;

            if (!int.TryParse(a, out m) || m < 1 || m > 12)
            {
                MessageBox.Show("Invalid Date", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            a = ExpDate.Substring(3, 2);
            if (!int.TryParse(a, out y) || currentY > y)
            {
                MessageBox.Show("The card has expired", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (currentY == y && currentM > m)
            {
                MessageBox.Show("The card has expired", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MessageBox.Show("Successful Purchase", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            model.CanPurchase = true;
            model.PurchaseCommandExecute(null);
            BackCommandExecute(null);

        }
        else if (PaypalCkecked)
        {
            try
            {
                Process.Start(new ProcessStartInfo { FileName = "https://www.paypal.com/us/signin", UseShellExecute = true });
                MessageBox.Show("Successful Purchase", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                model.CanPurchase = true;
                model.PurchaseCommandExecute(null);
                BackCommandExecute(null);
            }
            catch { }
        }
        else
        {
            MessageBox.Show("Successful Purchase", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            model.CanPurchase = true;
            model.PurchaseCommandExecute(null);
            BackCommandExecute(null);
        }


    }


    #endregion

    #endregion
}

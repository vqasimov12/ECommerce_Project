using ECommerce_Project.ViewModels.CommonViewModels;
using System.Windows.Input;

namespace ECommerce_Project.ViewModels.UserViewModels;

public class UserPaymentPageViewModel : BaseViewModel
{
    private bool secondPanelVisiblity = false;
    private bool firstPanelVisiblity = true;
    private bool deliveryChecked;
    private bool cardChecked;
    private bool paypalCkecked;

    public bool SecondPanelVisiblity { get => secondPanelVisiblity; set { secondPanelVisiblity = value; OnPropertyChanged(); } }
    public bool FirstPanelVisiblity { get => firstPanelVisiblity; set { firstPanelVisiblity = value; OnPropertyChanged(); } }

    public bool DeliveryChecked { get => deliveryChecked; set { deliveryChecked = value; OnPropertyChanged(); SecondPanelVisiblity = false; FirstPanelVisiblity = true; } }
    public bool CardChecked { get => cardChecked; set { cardChecked = value; OnPropertyChanged(); SecondPanelVisiblity = true; FirstPanelVisiblity = false; } }
    public bool PaypalCkecked { get => paypalCkecked; set { paypalCkecked = value; OnPropertyChanged(); SecondPanelVisiblity = false; FirstPanelVisiblity = true; } }


    public void RefreshDataSource()
    {
        SecondPanelVisiblity = false;
        FirstPanelVisiblity = true;
    }


    #region Commands






    #endregion
}

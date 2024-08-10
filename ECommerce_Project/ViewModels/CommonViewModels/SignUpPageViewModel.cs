using ECommerce_Project.Entity.Models;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using ECommerce_Project.Command;
using ECommerce_Project.Services;
using System.Windows.Controls;
using ECommerce_Project.Views.CommonViews;

namespace ECommerce_Project.ViewModels.CommonViewModels;
public class SignUpPageViewModel : BaseViewModel
{
    bool isOtpCodeVisible;
    private bool secondR;
    private string oTPCode;
    private User user = new();
    private bool emailIsReadonly = false;

    public User User { get => user; set { user = value; OnPropertyChanged(); } }
    public SignUpPageViewModel()
    {
        user = new();
        RegisterCommand = new RelayCommand(RegisterCommandExecute, CanRegisterButtonExecute);
        LoginCommand = new RelayCommand(loginCommandExecute);
    }

    #region LoginCommand
    public ICommand LoginCommand { get; set; }
    public void loginCommandExecute(object? obj)
    {
        Page? page = obj as Page;
        if (page is not null && page.NavigationService.CanGoBack)
        {
            User = new();
            IsOtpCodeVisible = false;
            OTPCode = "";
            SecondR = false;
            EmailIsReadonly = false;
            var navigatedPage = App.Container.GetInstance<LoginPageView>();
            var dataContext = App.Container.GetInstance<LoginPageViewModel>();
            dataContext.Email = "";
            dataContext.Password = "";
            navigatedPage.DataContext = dataContext;
            page.NavigationService.Navigate(navigatedPage);
        }
    }
    #endregion

    #region RegisterCommand

    bool SecondR { get => secondR; set { secondR = value; OnPropertyChanged(); } }
    public string OTPCode
    {
        get => oTPCode;
        set
        {
            oTPCode = value; OnPropertyChanged();
        }
    }
    public bool EmailIsReadonly { get => emailIsReadonly; set { emailIsReadonly = value; OnPropertyChanged(); } }
    public bool IsOtpCodeVisible { get => isOtpCodeVisible; set { isOtpCodeVisible = value; OnPropertyChanged(); } }
    public ICommand RegisterCommand { get; set; }
    public bool CanRegisterButtonExecute(object? obj)
    {
        return (Regex.IsMatch(User?.Name, @"^[a-zA-Z]{3,25}$") &&
            Regex.IsMatch(User?.Email, "^[a-zA-Z0-9.]+@[a-zA-Z0-9.]+\\.[a-zA-Z]{2,4}$") &&
            Regex.IsMatch(User?.Surname, @"^[a-zA-Z]{3,25}$") &&
            Regex.IsMatch(User?.Phone, @"(?:50|51|70|77|10|99|55|57) \d{3} \d{2} \d{2}") &&
            User?.Password?.Length > 3);
    }
    public void RegisterCommandExecute(object? obj)
    {
        if (!IsOtpCodeVisible)
        {
            if (CheckEmail(User?.Email!) is not null)
            {
                MessageBox.Show("This Email has already been used change another one", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                user.Email = "";
            }
            else if (SecondR == false)
            {
                IsOtpCodeVisible = true;
                MailService.SendMail(User?.Email!);
                SecondR = true;
                EmailIsReadonly = true;
            }
        }

        else
        {
            using var db = new AppDataContext();
            db.Users.Add(User);
            db.SaveChanges();
            User = new();
            IsOtpCodeVisible = false;
            OTPCode = "";
            SecondR = false;
            emailIsReadonly = false;
        }
    }

    #endregion
}

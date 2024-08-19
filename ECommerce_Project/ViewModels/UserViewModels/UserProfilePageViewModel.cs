using ECommerce_Project.Command;
using ECommerce_Project.Entity.Models;
using ECommerce_Project.Services;
using ECommerce_Project.ViewModels.CommonViewModels;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.Identity.Client.NativeInterop;
using Account = CloudinaryDotNet.Account;
namespace ECommerce_Project.ViewModels.UserViewModels;
public class UserProfilePageViewModel : BaseViewModel
{
    private User user1;
    private string password1;
    private string oTPCode;
    private bool secondR;
    private bool emailIsReadonly;
    private bool isOtpCodeVisible;
    public string Password1 { get => password1; set { password1 = value; OnPropertyChanged(); } }
    public User User1 { get => user1; set { user1 = value; OnPropertyChanged(); } }
     Cloudinary cloudinary { get; set; }
    Account account = new Account("doolsly8j", "445179498452818","SMlohF-hU9X8GBILv3FqTX_Q2Ok");

    public void RefreshDataSource()
    {
        using var db = new AppDataContext();
        User1 = db.Users.Include(z => z.Orders).ThenInclude(z => z.Products).ThenInclude(z => z.Product).ThenInclude(z => z.Category).Include(z => z.FavouriteProducts).ThenInclude(z => z.Category).Include(z => z.ShoppingCart).ThenInclude(z => z.Product).ThenInclude(z => z.Category).FirstOrDefault(z => z.Id == User1.Id)!;
        Password1 = User1.Password;
    }

    public UserProfilePageViewModel()
    {
        SelectImageCommand = new RelayCommand(SelectImageCommandExecute);
        SaveCommand = new RelayCommand(SaveCommandExecute, SaveCommandCanExecute);
        cloudinary = new Cloudinary(account);
    }

    #region Commands

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
        User1.Image = result;
    }

    #endregion

    #region SaveCommand

    public ICommand SaveCommand { get; set; }
    public bool SecondR { get => secondR; set { secondR = value; OnPropertyChanged(); } }
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
    public bool SaveCommandCanExecute(object? obj)
    {
        return (Regex.IsMatch(User1?.Name, @"^[a-zA-Z]{3,25}$") &&
            Regex.IsMatch(User1?.Email, "^[a-zA-Z0-9.]+@[a-zA-Z0-9.]+\\.[a-zA-Z]{2,4}$") &&
            Regex.IsMatch(User1?.Surname, @"^[a-zA-Z]{3,25}$") &&
            Regex.IsMatch(User1?.Phone, @"(?:50|51|70|77|10|99|55|57) \d{3} \d{2} \d{2}") &&
            User1?.Password?.Length > 3);
    }
    public void SaveCommandExecute(object? obj)
    {
        if (User1.Password != Password1)
        {
            MessageBox.Show("Passwords do not match", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        if (!IsOtpCodeVisible)
        {
            var a = CheckEmail(User1?.Email!);
            if (a is not null && a.Id != User1?.Id)
            {
                MessageBox.Show("This Email has already been used change another one", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                User1.Email = "";
            }
            else if (SecondR == false)
            {
                IsOtpCodeVisible = true;
                MailService.SendMail(User1?.Email!);
                SecondR = true;
                EmailIsReadonly = true;
            }
        }

        else
        {

            using var db = new AppDataContext();
            var user2 = db.Users.FirstOrDefault(z => z.Id == User1.Id)!;
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(User1.Image)
            };

            var uploadResult = cloudinary.Upload(uploadParams);
            string imageUrl = uploadResult.SecureUrl.ToString();
            User1.Image = imageUrl;
            user2.SetUser(User1);
            db.SaveChanges();
            IsOtpCodeVisible = false;
            OTPCode = "";
            SecondR = false;
            emailIsReadonly = false;
        }

        #endregion

    #endregion

    }
}

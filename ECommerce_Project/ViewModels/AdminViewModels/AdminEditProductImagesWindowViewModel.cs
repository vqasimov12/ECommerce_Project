using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using ECommerce_Project.Command;
using ECommerce_Project.ViewModels.CommonViewModels;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace ECommerce_Project.ViewModels.AdminViewModels;
public class AdminEditProductImagesWindowViewModel : BaseViewModel
{
    public ObservableCollection<string> ProductImages { get; set; } = new ObservableCollection<string>();
    private string _coverImage;
    private string _selectedImage;

    public string CoverImage { get => _coverImage; set { _coverImage = value; OnPropertyChanged(); } }
    public string SelectedImage { get => _selectedImage; set { _selectedImage = value; OnPropertyChanged(); } }


    public AdminEditProductImagesWindowViewModel()
    {
        AddImageCommand = new RelayCommand(AddImage);
        DeleteImageCommand = new RelayCommand(DeleteImageCommandExecute);
        SetCoverImageCommand = new RelayCommand(SetCoverImageCommandExecute, SetCoverImageCommandCanExecute);
        SaveCommand = new RelayCommand(SaveCommandExecute);
    }

    #region Commands

    #region AddImageCommand
    public ICommand AddImageCommand { get; set; }

    private void AddImage(object? obj)
    {
        System.Windows.Forms.OpenFileDialog dlg = new System.Windows.Forms.OpenFileDialog();
        dlg.Filter = "Image files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg";
        dlg.ShowDialog();
        string? result = dlg.FileName;
        if (string.IsNullOrEmpty(result))
            return;
        if (ProductImages.Any(z => z == result))
        {
            var dlgResult = MessageBox.Show("This item already exists in the list. Do you want to add it again?", "Duplicate Item", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (dlgResult == MessageBoxResult.Yes)
                ProductImages.Add(result);

        }
        else
            ProductImages.Add(result);
        if (ProductImages.Count == 1)
            CoverImage = ProductImages[0];
    }
    #endregion

    #region DeleteImageCommand
    public ICommand DeleteImageCommand { get; set; }

    private void DeleteImageCommandExecute(object? obj)
    {
        if (obj is string imagePath && ProductImages.Contains(imagePath))
        {
            ProductImages.Remove(imagePath);
            if (CoverImage == imagePath)
            {
                CoverImage = ProductImages.FirstOrDefault();

            }
        }
    }
    #endregion

    #region SetCoverImageCommand

    public ICommand SetCoverImageCommand { get; set; }

    public bool SetCoverImageCommandCanExecute(object? obj)
    {
        if (obj is not string imagePath)
            return false;
        return CoverImage != imagePath;
    }
    private void SetCoverImageCommandExecute(object? obj)
    {
        if (obj is string imagePath && ProductImages.Contains(imagePath))
            CoverImage = imagePath;
    }

    #endregion

    #region SaveCommand
    public ICommand SaveCommand { get; set; }
    public void SaveCommandExecute(object? obj)
    {
        var temp = new List<string>();
        var data = App.Container.GetInstance<AdminProductPageViewModel>();
        if (ProductImages.Count == 0)
        {
            ProductImages.Add("https://res.cloudinary.com/doolsly8j/image/upload/v1724926263/nm2zhsf8uqfwdwjehkex.png");
            CoverImage = @"https://res.cloudinary.com/doolsly8j/image/upload/v1724926263/nm2zhsf8uqfwdwjehkex.png";
        }
        else
        {
            temp = new(ProductImages);
            Account account = new Account("doolsly8j", "445179498452818", "SMlohF-hU9X8GBILv3FqTX_Q2Ok");
            Cloudinary cloudinary = new(account);
            new Thread(() =>
            {
                for (int i = 0; i < ProductImages.Count; i++)
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(temp[i])
                    };

                    var uploadResult = cloudinary.Upload(uploadParams);
                    temp[i] = uploadResult.SecureUrl.ToString();
                }
                data.Product.Images = new(temp);
            }).Start();
        }
        data.Product.CoverImage = CoverImage;
        Close(obj);
    }
    #endregion

    #endregion
}

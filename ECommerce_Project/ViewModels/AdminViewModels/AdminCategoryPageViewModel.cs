using ECommerce_Project.Command;
using ECommerce_Project.Entity.Models;
using ECommerce_Project.ViewModels.CommonViewModels;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace ECommerce_Project.ViewModels.AdminViewModels;
public class AdminCategoryPageViewModel : BaseViewModel
{
    private ObservableCollection<Category> categories = [];
    private bool textVisiblity;
    private string newCategory = "";

    public string NewCategory { get => newCategory; set { newCategory = value; OnPropertyChanged(); } }
    public bool TextVisiblity { get => textVisiblity; set { textVisiblity = value; OnPropertyChanged(); } }
    public ObservableCollection<Category> Categories { get => categories; set { categories = value; OnPropertyChanged(); } }
    public AdminCategoryPageViewModel()
    {
        AddCommand = new RelayCommand(AddCommandExecute, AddCommandCanExecute);
        EditCommand = new RelayCommand(EditCommandExecute, EditDeleteCommandsCanExecute);
        DeleteCommand = new RelayCommand(DeleteCommandExecute, EditDeleteCommandsCanExecute);

        RefreshCategories();
    }
 
    public void RefreshCategories()
    {
        Categories = new();
        using var db = new AppDataContext();
        TextVisiblity = false;
        foreach (var i in db.Categories)
            Categories.Add(i);
    }

    #region Commands

    #region AddCommand
    public ICommand AddCommand { get; set; }
    public bool AddCommandCanExecute(object? obj)
    {
        if (!TextVisiblity)
            return true;
        else
            return NewCategory.Length >= 3;

    }
    public void AddCommandExecute(object? obj)
    {
        if (!TextVisiblity)
        {
            TextVisiblity = true;
            return;
        }
        using var db = new AppDataContext();
        db.Categories.Add(new Category { Name = NewCategory });
        db.SaveChanges();
        RefreshCategories();
        NewCategory = "";
    }

    #endregion

    #region EditCommand
    public ICommand EditCommand { get; set; }
    public bool EditDeleteCommandsCanExecute(object? obj)
    {
        var listbox = obj as ListBox;
        return listbox?.SelectedItem is not null;
    }
    public void EditCommandExecute(object? obj)
    {
        var listbox = obj as ListBox;
        var category = listbox?.SelectedItem as Category;
        if (category is null) return;
        if (!TextVisiblity)
        {
            TextVisiblity = true;
            NewCategory = category?.Name;
            return;
        }
        using var db = new AppDataContext();
        int id = category.Id;
        var cat = db.Categories.FirstOrDefault(x => x.Id == id);
        cat.Name = NewCategory;
        db.SaveChanges();
        RefreshCategories();
    }

    #endregion

    #region DeleteCommand

    public ICommand DeleteCommand { get; set; }
    public void DeleteCommandExecute(object? obj)
    {
        var listbox = obj as ListBox;
        var category = listbox?.SelectedItem as Category;
        if (category is null) return;
        using var db = new AppDataContext();
        int id = category.Id;
        foreach (var i in db.Categories)
            if (i.Id == id)
            {
                db.Categories.Remove(i);
                break;
            }
        db.SaveChanges();
        RefreshCategories();
    }

    #endregion

    #endregion

}

using ECommerce_Project.Services;
using System.Text.RegularExpressions;

namespace ECommerce_Project.Entity.Abstractions;
public class Person : NotifyPropertyChangedService
{
    private int id;
    private string? name;
    private string? surname;
    private string? email;
    private string? address;
    private string? phone;
    private string? password;
    private string? image;
    public Person()
    {
        Name = "";
        Surname = "";
        Email = "";
        Address = "";
        Phone = "";
        Password = "";
    }
    public int Id { get => id; set { id = value; OnPropertyChanged(); } }
    public string? Name { get => name; set { name = value; OnPropertyChanged(); } }
    public string? Image { get => image; set { image = value; OnPropertyChanged(); } }
    public string? Surname { get => surname; set { surname = value; OnPropertyChanged(); } }
    public string? Email { get => email; set { email = value; OnPropertyChanged(); } }
    public string? Address { get => address; set { address = value; OnPropertyChanged(); } }
    public string? Phone
    {
        get => phone;
        set
        {
            if (Regex.IsMatch(value, @"^[0-9]{0,10}$") || string.IsNullOrEmpty(value))
            {

                phone = value;
                OnPropertyChanged();
            }
        }
    }

    public string? Password { get => password; set { password = value; OnPropertyChanged(); } }
}


using ECommerce_Project.Services;

namespace ECommerce_Project.Entity.Models;

public class Category:NotifyPropertyChangedService
{
    private string name="";

    public int Id { get; set; }
    public string Name { get => name; set { name = value; OnPropertyChanged(); }}
    public override string ToString()
    {
        return Name;
    }

}

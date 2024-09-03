using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Windows.Forms;

namespace ECommerce_Project.Entity.Models;
public class AppDataContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<ProductView> ProductViews { get; set; }
    public AppDataContext()
    {
        //Database.EnsureDeleted();
        if (!Database.CanConnect())
        {
            Database.EnsureCreated();
            var user = new User();
            user.Address = "Shamkir";
            user.Name = "Vaqif";
            user.Surname = "Qasimov";
            user.Email = "qasimov.vaqif512@gmail.com";
            user.Password = "user123";
            user.Phone = "777777777";
            Users.Add(user);
            var cat = new Category();
            cat.Name = "Category1";
            Categories.Add(cat);
            SaveChanges();
            var prod = new Product
            {
                ProductName = "Product1",
                ProductDescription = "Desciption1",
                RateCount = 1,
                Images = [],
                RatingView = 5,
                Quantity = 3,
                Category = cat,
                Price = 10
            };
            prod.LikeImage = "../../Icons/Like.png";
            Products.Add(prod);
            user.FavouriteProducts.Add(prod);
            var pv = new ProductView { Count = 1, Product = prod };
            ProductViews.Add(pv);
            user.ShoppingCart.Add(pv);
            SaveChanges();
        }
        else
            Database.EnsureCreated();

    }
 
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = App.GetConnectionString();
        optionsBuilder.UseSqlServer(connectionString);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>().HasIndex(x => x.Name).IsUnique();
        modelBuilder.Entity<User>().HasIndex(x => x.Email).IsUnique();


    }
}

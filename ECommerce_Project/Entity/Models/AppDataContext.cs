﻿using Microsoft.EntityFrameworkCore;

namespace ECommerce_Project.Entity.Models;
public class AppDataContext:DbContext
{
    public DbSet<User>Users { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Category>Categories { get; set; }
    public AppDataContext()
    {
        //Database.EnsureDeleted();
        Database.EnsureCreated(); 
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ECommerce;Integrated Security=True;");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>().HasIndex(x => x.Name).IsUnique(); 
    }
}
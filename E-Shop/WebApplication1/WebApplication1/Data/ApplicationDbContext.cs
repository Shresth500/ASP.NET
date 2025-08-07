using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;


namespace WebApplication1.Data;


public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<User> User { get; set; }
    public DbSet<Product> Product { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }
    public DbSet<Cart> Cart {  get; set; }
    public DbSet<Order> Order { get; set; }
    public DbSet<Address> Address { get; set; }
    public DbSet<OrderItems> OrderItem { get; set; }
    public DbSet<Notification> Notification { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>().HasIndex(a => a.Email).IsUnique();
        modelBuilder.Entity<Order>().HasKey(a => a.Id);
        modelBuilder.Entity<Cart>().HasKey(c => new { c.UserId, c.ProductId });
        modelBuilder.Entity<OrderItems>().HasKey(a => new { a.OrderId, a.ProductId });

        modelBuilder.Entity
            <Product>().HasOne(a => a.User)
            .WithMany(b => b.Products)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ProductImage>()
            .HasOne(a => a.Product)
            .WithMany(b => b.ProductImages)
            .HasForeignKey(c => c.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Cart>()
            .HasOne(a => a.User)
            .WithMany(b => b.Cart)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Cart>()
            .HasOne(a => a.Product)
            .WithMany(b => b.Cart)
            .HasForeignKey(c => c.ProductId)
            .OnDelete(DeleteBehavior.Restrict);


        modelBuilder.Entity<OrderItems>()
            .HasOne(a => a.Order)
            .WithMany(b => b.OrderItems)
            .HasForeignKey(c => c.OrderId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<OrderItems>()
            .HasOne(a => a.Product)
            .WithMany(b => b.OrderItem)
            .HasForeignKey(c => c.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Order>()
            .HasOne(a => a.User)
            .WithMany(b => b.Order)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Notification>()
            .HasOne(a => a.User)
            .WithMany(b => b.Notifications)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Address>()
            .HasOne(a => a.User)
            .WithMany(b => b.AddressList)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

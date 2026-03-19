using Microsoft.EntityFrameworkCore;

namespace SportShopV1.Models;

public class SportShopContext : DbContext
{
    public DbSet<Role> Roles { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Manufacturer> Manufacturers { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<OrderStatus> OrderStatuses { get; set; }
    public DbSet<PickupPoint> PickupPoints { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=sport_shop;Username=postgres;Password=postgres");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>(e =>
        {
            e.ToTable("roles");
            e.Property(r => r.Id).HasColumnName("id");
            e.Property(r => r.Name).HasColumnName("name");
        });

        modelBuilder.Entity<User>(e =>
        {
            e.ToTable("users");
            e.Property(u => u.Id).HasColumnName("id");
            e.Property(u => u.Login).HasColumnName("login");
            e.Property(u => u.PasswordText).HasColumnName("password_text");
            e.Property(u => u.FullName).HasColumnName("full_name");
            e.Property(u => u.RoleId).HasColumnName("role_id");
            e.HasOne(u => u.Role).WithMany(r => r.Users).HasForeignKey(u => u.RoleId);
        });

        modelBuilder.Entity<Category>(e =>
        {
            e.ToTable("categories");
            e.Property(c => c.Id).HasColumnName("id");
            e.Property(c => c.Name).HasColumnName("name");
        });

        modelBuilder.Entity<Manufacturer>(e =>
        {
            e.ToTable("manufacturers");
            e.Property(m => m.Id).HasColumnName("id");
            e.Property(m => m.Name).HasColumnName("name");
        });

        modelBuilder.Entity<Supplier>(e =>
        {
            e.ToTable("suppliers");
            e.Property(s => s.Id).HasColumnName("id");
            e.Property(s => s.Name).HasColumnName("name");
        });

        modelBuilder.Entity<Product>(e =>
        {
            e.ToTable("products");
            e.Property(p => p.Id).HasColumnName("id");
            e.Property(p => p.Article).HasColumnName("article");
            e.Property(p => p.Name).HasColumnName("name");
            e.Property(p => p.Unit).HasColumnName("unit");
            e.Property(p => p.Price).HasColumnName("price");
            e.Property(p => p.SupplierId).HasColumnName("supplier_id");
            e.Property(p => p.ManufacturerId).HasColumnName("manufacturer_id");
            e.Property(p => p.CategoryId).HasColumnName("category_id");
            e.Property(p => p.DiscountPct).HasColumnName("discount_pct");
            e.Property(p => p.StockQty).HasColumnName("stock_qty");
            e.Property(p => p.Description).HasColumnName("description");
            e.Property(p => p.Photo).HasColumnName("photo");
            e.Ignore(p => p.PriceDiscounted);
            e.HasOne(p => p.Category).WithMany(c => c.Products).HasForeignKey(p => p.CategoryId);
            e.HasOne(p => p.Manufacturer).WithMany(m => m.Products).HasForeignKey(p => p.ManufacturerId);
            e.HasOne(p => p.Supplier).WithMany(s => s.Products).HasForeignKey(p => p.SupplierId);
        });

        modelBuilder.Entity<OrderStatus>(e =>
        {
            e.ToTable("order_statuses");
            e.Property(s => s.Id).HasColumnName("id");
            e.Property(s => s.Name).HasColumnName("name");
        });

        modelBuilder.Entity<PickupPoint>(e =>
        {
            e.ToTable("pickup_points");
            e.Property(pp => pp.Id).HasColumnName("id");
            e.Property(pp => pp.Address).HasColumnName("address");
            e.Property(pp => pp.Phone).HasColumnName("phone");
        });

        modelBuilder.Entity<Order>(e =>
        {
            e.ToTable("orders");
            e.Property(o => o.Id).HasColumnName("id");
            e.Property(o => o.OrderDate).HasColumnName("order_date");
            e.Property(o => o.DeliveryDate).HasColumnName("delivery_date");
            e.Property(o => o.PickupPointId).HasColumnName("pickup_point_id");
            e.Property(o => o.ClientName).HasColumnName("client_name");
            e.Property(o => o.PickupCode).HasColumnName("pickup_code");
            e.Property(o => o.StatusId).HasColumnName("status_id");
            e.HasOne(o => o.PickupPoint).WithMany(pp => pp.Orders).HasForeignKey(o => o.PickupPointId);
            e.HasOne(o => o.Status).WithMany(s => s.Orders).HasForeignKey(o => o.StatusId);
        });

        modelBuilder.Entity<OrderItem>(e =>
        {
            e.ToTable("order_items");
            e.Property(oi => oi.Id).HasColumnName("id");
            e.Property(oi => oi.OrderId).HasColumnName("order_id");
            e.Property(oi => oi.ProductId).HasColumnName("product_id");
            e.Property(oi => oi.Quantity).HasColumnName("quantity");
            e.HasOne(oi => oi.Order).WithMany(o => o.OrderItems).HasForeignKey(oi => oi.OrderId);
            e.HasOne(oi => oi.Product).WithMany(p => p.OrderItems).HasForeignKey(oi => oi.ProductId);
        });
    }
}

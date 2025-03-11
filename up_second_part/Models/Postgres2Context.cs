using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace up_second_part.Models;

public partial class Postgres2Context : DbContext
{
    public Postgres2Context()
    {
    }

    public Postgres2Context(DbContextOptions<Postgres2Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Manufacturer> Manufacturers { get; set; }

    public virtual DbSet<MeasurementUnit> MeasurementUnits { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderProduct> OrderProducts { get; set; }

    public virtual DbSet<OrderStatus> OrderStatuses { get; set; }

    public virtual DbSet<PickupPoint> PickupPoints { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductCategory> ProductCategories { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=postgres2;Username=postgres;Password=12345");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Manufacturer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("manufacturers_pk");

            entity.ToTable("manufacturers");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ManufacturerName).HasColumnName("manufacturer_name");
        });

        modelBuilder.Entity<MeasurementUnit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("measurement_units_pk");

            entity.ToTable("measurement_units");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.MeasurementUnitName).HasColumnName("measurement_unit_name");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("newtable_pk");

            entity.ToTable("orders");

            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.OrderClient)
                .HasDefaultValue(0)
                .HasColumnName("order_client");
            entity.Property(e => e.OrderDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("order_date");
            entity.Property(e => e.OrderDeliveryDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("order_delivery_date");
            entity.Property(e => e.OrderPickupPoint)
                .ValueGeneratedOnAdd()
                .HasColumnName("order_pickup_point");
            entity.Property(e => e.OrderReceiptCode).HasColumnName("order_receipt_code");
            entity.Property(e => e.OrderStatus)
                .ValueGeneratedOnAdd()
                .HasColumnName("order_status");

            entity.HasOne(d => d.OrderClientNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.OrderClient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orders_user_fk");

            entity.HasOne(d => d.OrderPickupPointNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.OrderPickupPoint)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orders_pickup_points_fk");

            entity.HasOne(d => d.OrderStatusNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.OrderStatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orders_order_statuses_fk");
        });

        modelBuilder.Entity<OrderProduct>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.ProductArticleNumber }).HasName("order_product_pkey");

            entity.ToTable("order_product");

            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.ProductArticleNumber)
                .HasMaxLength(100)
                .HasColumnName("product_article_number");
            entity.Property(e => e.ProductCount).HasColumnName("product_count");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderProducts)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("order_product_order_id_fkey");

            entity.HasOne(d => d.ProductArticleNumberNavigation).WithMany(p => p.OrderProducts)
                .HasForeignKey(d => d.ProductArticleNumber)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("order_product_product_article_number_fkey");
        });

        modelBuilder.Entity<OrderStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("order_statuses_pk");

            entity.ToTable("order_statuses");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.StatusName).HasColumnName("status_name");
        });

        modelBuilder.Entity<PickupPoint>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pickup_points_pk");

            entity.ToTable("pickup_points");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PickupPointAddress).HasColumnName("pickup_point_address");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductArticleNumber).HasName("products_pk");

            entity.ToTable("products");

            entity.Property(e => e.ProductArticleNumber)
                .HasMaxLength(100)
                .HasColumnName("product_article_number");
            entity.Property(e => e.ProductCategory)
                .ValueGeneratedOnAdd()
                .HasColumnName("product_category");
            entity.Property(e => e.ProductCost)
                .HasPrecision(19, 4)
                .HasColumnName("product_cost");
            entity.Property(e => e.ProductDescription).HasColumnName("product_description");
            entity.Property(e => e.ProductDiscountAmount).HasColumnName("product_discount_amount");
            entity.Property(e => e.ProductManufacturer)
                .ValueGeneratedOnAdd()
                .HasColumnName("product_manufacturer");
            entity.Property(e => e.ProductMaxDiscountAmount).HasColumnName("product_max_discount_amount");
            entity.Property(e => e.ProductMeasurementUnit)
                .ValueGeneratedOnAdd()
                .HasColumnName("product_measurement_unit");
            entity.Property(e => e.ProductName).HasColumnName("product_name");
            entity.Property(e => e.ProductPhoto)
                .HasMaxLength(100)
                .HasColumnName("product_photo");
            entity.Property(e => e.ProductQuantityInStock).HasColumnName("product_quantity_in_stock");
            entity.Property(e => e.ProductStatus).HasColumnName("product_status");
            entity.Property(e => e.ProductSupplier)
                .ValueGeneratedOnAdd()
                .HasColumnName("product_supplier");

            entity.HasOne(d => d.ProductCategoryNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProductCategory)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("products_product_category_fk");

            entity.HasOne(d => d.ProductManufacturerNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProductManufacturer)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("products_manufacturers_fk");

            entity.HasOne(d => d.ProductMeasurementUnitNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProductMeasurementUnit)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("products_measurement_units_fk");

            entity.HasOne(d => d.ProductSupplierNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProductSupplier)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("products_suppliers_fk");
        });

        modelBuilder.Entity<ProductCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("product_category_pk");

            entity.ToTable("product_category");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CategoryName).HasColumnName("category_name");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("role_pkey");

            entity.ToTable("role");

            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.RoleName)
                .HasMaxLength(100)
                .HasColumnName("role_name");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("suppliers_pk");

            entity.ToTable("suppliers");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.SupplierName).HasColumnName("supplier_name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("user_pkey");

            entity.ToTable("user");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.UserLogin).HasColumnName("user_login");
            entity.Property(e => e.UserName)
                .HasMaxLength(100)
                .HasColumnName("user_name");
            entity.Property(e => e.UserPassword).HasColumnName("user_password");
            entity.Property(e => e.UserPatronymic)
                .HasMaxLength(100)
                .HasColumnName("user_patronymic");
            entity.Property(e => e.UserRole).HasColumnName("user_role");
            entity.Property(e => e.UserSurname)
                .HasMaxLength(100)
                .HasColumnName("user_surname");

            entity.HasOne(d => d.UserRoleNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.UserRole)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_user_role_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

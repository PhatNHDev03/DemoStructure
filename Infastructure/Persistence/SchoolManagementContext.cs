using System;
using System.Collections.Generic;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infastructure.Persistence;

public partial class SchoolManagementContext : DbContext
{
    public SchoolManagementContext()
    {
    }

    public SchoolManagementContext(DbContextOptions<SchoolManagementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Class> Classes { get; set; }

    public virtual DbSet<Inventoryproduct> Inventoryproducts { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<SystemAccount> SystemAccounts { get; set; }

    public virtual DbSet<TransactionTable> TransactionTables { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=school_management;Username=postgres;Password=123456");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Class>(entity =>
        {
            entity.HasKey(e => e.ClassId).HasName("class_pkey");

            entity.ToTable("class");

            entity.HasIndex(e => e.ClassCode, "class_class_code_key").IsUnique();

            entity.HasIndex(e => e.ClassCode, "ix_class_class_code");

            entity.Property(e => e.ClassId).HasColumnName("class_id");
            entity.Property(e => e.ClassCode)
                .HasMaxLength(20)
                .HasColumnName("class_code");
            entity.Property(e => e.ClassName)
                .HasMaxLength(100)
                .HasColumnName("class_name");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.MaxStudents)
                .HasDefaultValue(30)
                .HasColumnName("max_students");
            entity.Property(e => e.RoomNumber)
                .HasMaxLength(20)
                .HasColumnName("room_number");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValueSql("'Active'::character varying")
                .HasColumnName("status");
            entity.Property(e => e.TeacherName)
                .HasMaxLength(100)
                .HasColumnName("teacher_name");
        });

        modelBuilder.Entity<Inventoryproduct>(entity =>
        {
            entity.HasKey(e => e.InventoryId).HasName("inventoryproduct_pkey");

            entity.ToTable("inventoryproduct");

            entity.HasIndex(e => e.SystemAccountId, "ix_inventory_account_id");

            entity.HasIndex(e => e.ProductId, "ix_inventory_product_id");

            entity.Property(e => e.InventoryId).HasColumnName("inventory_id");
            entity.Property(e => e.LastUpdated)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("last_updated");
            entity.Property(e => e.Notes)
                .HasMaxLength(255)
                .HasColumnName("notes");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Quantity)
                .HasDefaultValue(0)
                .HasColumnName("quantity");
            entity.Property(e => e.SystemAccountId).HasColumnName("system_account_id");
            entity.Property(e => e.WarehouseLocation)
                .HasMaxLength(50)
                .HasColumnName("warehouse_location");

            entity.HasOne(d => d.Product).WithMany(p => p.Inventoryproducts)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("fk_inventory_product");

            entity.HasOne(d => d.SystemAccount).WithMany(p => p.Inventoryproducts)
                .HasForeignKey(d => d.SystemAccountId)
                .HasConstraintName("fk_inventory_account");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("product_pkey");

            entity.ToTable("product");

            entity.HasIndex(e => e.ProductCode, "ix_product_product_code");

            entity.HasIndex(e => e.ProductName, "ix_product_product_name");

            entity.HasIndex(e => e.ProductCode, "product_product_code_key").IsUnique();

            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Category)
                .HasMaxLength(50)
                .HasColumnName("category");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("description");
            entity.Property(e => e.Price)
                .HasPrecision(18, 2)
                .HasColumnName("price");
            entity.Property(e => e.ProductCode)
                .HasMaxLength(20)
                .HasColumnName("product_code");
            entity.Property(e => e.ProductName)
                .HasMaxLength(100)
                .HasColumnName("product_name");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValueSql("'Active'::character varying")
                .HasColumnName("status");
            entity.Property(e => e.Unit)
                .HasMaxLength(20)
                .HasDefaultValueSql("'Cái'::character varying")
                .HasColumnName("unit");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("student_pkey");

            entity.ToTable("student");

            entity.HasIndex(e => e.ClassId, "ix_student_class_id");

            entity.HasIndex(e => e.FullName, "ix_student_full_name");

            entity.HasIndex(e => e.StudentCode, "student_student_code_key").IsUnique();

            entity.Property(e => e.StudentId).HasColumnName("student_id");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasColumnName("address");
            entity.Property(e => e.ClassId).HasColumnName("class_id");
            entity.Property(e => e.DateOfBirth).HasColumnName("date_of_birth");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.EnrollmentDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("enrollment_date");
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .HasColumnName("full_name");
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .HasColumnName("phone");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValueSql("'Active'::character varying")
                .HasColumnName("status");
            entity.Property(e => e.StudentCode)
                .HasMaxLength(20)
                .HasColumnName("student_code");

            entity.HasOne(d => d.Class).WithMany(p => p.Students)
                .HasForeignKey(d => d.ClassId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_student_class");
        });

        modelBuilder.Entity<SystemAccount>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("system_account_pkey");

            entity.ToTable("system_account");

            entity.HasIndex(e => e.Role, "ix_system_account_role");

            entity.HasIndex(e => e.Username, "ix_system_account_username");

            entity.HasIndex(e => e.Username, "system_account_username_key").IsUnique();

            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .HasColumnName("full_name");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .HasColumnName("password_hash");
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .HasDefaultValueSql("'User'::character varying")
                .HasColumnName("role");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValueSql("'Active'::character varying")
                .HasColumnName("status");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");
        });

        modelBuilder.Entity<TransactionTable>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("transaction_table_pkey");

            entity.ToTable("transaction_table");

            entity.HasIndex(e => e.SystemAccountId, "ix_transaction_account_id");

            entity.HasIndex(e => e.InventoryId, "ix_transaction_inventory_id");

            entity.HasIndex(e => e.Status, "ix_transaction_status");

            entity.Property(e => e.TransactionId).HasColumnName("transaction_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.InventoryId).HasColumnName("inventory_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValueSql("'Pending'::character varying")
                .HasColumnName("status");
            entity.Property(e => e.SystemAccountId).HasColumnName("system_account_id");

            entity.HasOne(d => d.Inventory).WithMany(p => p.TransactionTables)
                .HasForeignKey(d => d.InventoryId)
                .HasConstraintName("fk_transaction_inventory");

            entity.HasOne(d => d.SystemAccount).WithMany(p => p.TransactionTables)
                .HasForeignKey(d => d.SystemAccountId)
                .HasConstraintName("fk_transaction_account");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

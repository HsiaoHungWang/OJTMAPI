using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace OJTMAPI.Models;

public partial class ClassDbContext : DbContext
{
    public ClassDbContext()
    {
    }

    public ClassDbContext(DbContextOptions<ClassDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Member> Members { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }

    public virtual DbSet<Spot> Spots { get; set; }

    public virtual DbSet<SpotImage> SpotImages { get; set; }

    public virtual DbSet<SpotImagesSpot> SpotImagesSpots { get; set; }

    public virtual DbSet<SpotsCategory> SpotsCategories { get; set; }

    public virtual DbSet<TodoItem> TodoItems { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=ClassDB;Integrated Security=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.ToTable("Address");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.City)
                .HasMaxLength(10)
                .HasColumnName("city");
            entity.Property(e => e.Road)
                .HasMaxLength(200)
                .HasColumnName("road");
            entity.Property(e => e.SiteId)
                .HasMaxLength(50)
                .HasColumnName("site_id");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.Property(e => e.CategoryName).HasMaxLength(50);
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CourseImage).HasMaxLength(200);
            entity.Property(e => e.CourseName).HasMaxLength(200);
            entity.Property(e => e.DaysOfWeek).HasMaxLength(50);
            entity.Property(e => e.Location).HasMaxLength(50);
            entity.Property(e => e.TimePeriods).HasMaxLength(50);

            entity.HasOne(d => d.Category).WithMany(p => p.Courses)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_Courses_Categories");
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.Property(e => e.Address).HasMaxLength(200);
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.FileName).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(256);
            entity.Property(e => e.Salt).HasMaxLength(256);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.Property(e => e.OrderId)
                .ValueGeneratedNever()
                .HasColumnName("OrderID");
            entity.Property(e => e.MemberId).HasColumnName("MemberID");
            entity.Property(e => e.OrderDate).HasColumnType("datetime");

            entity.HasOne(d => d.Member).WithMany(p => p.Orders)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_Members");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.CourseId });

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.CoursePrice).HasColumnType("money");

            entity.HasOne(d => d.Course).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderDetails_Courses");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderDetails_Orders");
        });

        modelBuilder.Entity<ShoppingCart>(entity =>
        {
            entity.HasKey(e => e.RecordId);

            entity.ToTable("ShoppingCart");

            entity.Property(e => e.RecordId).HasColumnName("RecordID");
            entity.Property(e => e.CartId)
                .HasMaxLength(50)
                .HasColumnName("CartID");
            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.DateCreated).HasColumnType("datetime");
        });

        modelBuilder.Entity<Spot>(entity =>
        {
            entity.Property(e => e.SpotId).ValueGeneratedNever();
            entity.Property(e => e.Address).HasMaxLength(200);
            entity.Property(e => e.DateCreated).HasColumnType("datetime");
            entity.Property(e => e.Latitude).HasMaxLength(20);
            entity.Property(e => e.Longitude).HasMaxLength(20);
            entity.Property(e => e.SpotTitle).HasMaxLength(50);

            entity.HasOne(d => d.Category).WithMany(p => p.Spots)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_Spots_Spots");
        });

        modelBuilder.Entity<SpotImage>(entity =>
        {
            entity.HasKey(e => e.ImageId);

            entity.Property(e => e.ImageId).ValueGeneratedNever();

            entity.HasOne(d => d.Spot).WithMany(p => p.SpotImages)
                .HasForeignKey(d => d.SpotId)
                .HasConstraintName("FK_SpotImages_Spots");
        });

        modelBuilder.Entity<SpotImagesSpot>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("SpotImagesSpot");

            entity.Property(e => e.Address).HasMaxLength(200);
            entity.Property(e => e.DateCreated).HasColumnType("datetime");
            entity.Property(e => e.Latitude).HasMaxLength(20);
            entity.Property(e => e.Longitude).HasMaxLength(20);
            entity.Property(e => e.SpotTitle).HasMaxLength(50);
        });

        modelBuilder.Entity<SpotsCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId);

            entity.Property(e => e.CategoryId).ValueGeneratedNever();
            entity.Property(e => e.CategoryName).HasMaxLength(50);
        });

        modelBuilder.Entity<TodoItem>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

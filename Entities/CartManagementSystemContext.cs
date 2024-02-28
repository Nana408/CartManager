using Microsoft.EntityFrameworkCore;

namespace CartManagmentSystem.Entities;

public partial class CartManagementSystemContext : DbContext
{
    public CartManagementSystemContext()
    {
    }

    public CartManagementSystemContext(DbContextOptions<CartManagementSystemContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Application> Applications { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<CartItem> CartItems { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserLog> UserLogs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);

            IConfiguration configuration = builder.Build();
            string conString = configuration.GetValue<string>("ConnectionStrings:CartManagerEntities");

            optionsBuilder.UseSqlServer(conString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Application>(entity =>
        {
            entity.ToTable("Application");

            entity.Property(e => e.CreatedBy).HasMaxLength(256);
            entity.Property(e => e.DateCreated).HasColumnType("datetime");
            entity.Property(e => e.DateModified).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(256);
            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.SourceKey).HasMaxLength(256);
            entity.Property(e => e.SourceToken).HasMaxLength(256);
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.CartId).HasName("PK__Cart__51BCD797D863EEA0");

            entity.ToTable("Cart");

            entity.Property(e => e.CartId).HasColumnName("CartID");
            entity.Property(e => e.CreationBy).HasMaxLength(100);
            entity.Property(e => e.CreationDate).HasColumnType("datetime");
            entity.Property(e => e.LastUpdatedBy).HasMaxLength(100);
            entity.Property(e => e.LastUpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Carts)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Cart__UserID__286302EC");
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => e.CartItemId).HasName("PK__CartItem__488B0B2A7BB4D20E");

            entity.Property(e => e.CartItemId).HasColumnName("CartItemID");
            entity.Property(e => e.CartId).HasColumnName("CartID");
            entity.Property(e => e.CreationBy).HasMaxLength(100);
            entity.Property(e => e.CreationDate).HasColumnType("datetime");
            entity.Property(e => e.LastUpdatedBy).HasMaxLength(100);
            entity.Property(e => e.LastUpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");

            entity.HasOne(d => d.Cart).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.CartId)
                .HasConstraintName("FK__CartItems__CartI__2B3F6F97");

            entity.HasOne(d => d.Product).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__CartItems__Produ__2C3393D0");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Products__B40CC6ED8038BA28");

            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.CreationBy).HasMaxLength(100);
            entity.Property(e => e.CreationDate).HasColumnType("datetime");
            entity.Property(e => e.LastUpdatedBy).HasMaxLength(100);
            entity.Property(e => e.LastUpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ProductName).HasMaxLength(100);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCACB32849E6");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.CreationBy).HasMaxLength(100);
            entity.Property(e => e.CreationDate).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.LastUpdatedBy).HasMaxLength(100);
            entity.Property(e => e.LastUpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.Password).HasMaxLength(100);
            entity.Property(e => e.Phonenumber).HasMaxLength(100);
            entity.Property(e => e.Username).HasMaxLength(100);
        });

        modelBuilder.Entity<UserLog>(entity =>
        {
            entity.ToTable("UserLog");

            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.SourceId).HasColumnName("SourceID");
            entity.Property(e => e.SourceName).HasMaxLength(255);
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.UserAgent).HasMaxLength(255);
            entity.Property(e => e.UserFunction).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

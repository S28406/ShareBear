using PRO.Models;

namespace PRO.Data.Context;

using Microsoft.EntityFrameworkCore;

public class ToolLendingContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Tool> Tools { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<ToolAccessory> ToolAccessories { get; set; }
    public DbSet<Schedule> Schedules { get; set; }
    public DbSet<Borrow> Borrows { get; set; }
    public DbSet<ProductBorrow> ProductBorrows { get; set; }
    public DbSet<Return> Returns { get; set; }
    public DbSet<SecurityDeposit> SecurityDeposits { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<History> Histories { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<LendingPartner> LendingPartners { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductBorrow>()
            .HasOne(pb => pb.Borrow)
            .WithMany(b => b.ProductBorrows)
            .HasForeignKey(pb => pb.BorrowId);

        modelBuilder.Entity<ProductBorrow>()
            .HasOne(pb => pb.Tool)
            .WithMany()
            .HasForeignKey(pb => pb.ToolId);
        
        modelBuilder.Entity<Payment>(e =>
        {
            e.HasOne(p => p.Borrow)
                .WithMany(b => b.Payments)
                .HasForeignKey(p => p.OrdersId)         
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<User>()
            .HasMany(u => u.Tools)
            .WithOne(t => t.User)
            .HasForeignKey(t => t.UsersId);
        
        modelBuilder.Entity<User>()
            .HasMany(u => u.Borrows)
            .WithOne(b => b.User)
            .HasForeignKey(b => b.UsersId);
        
        modelBuilder.Entity<User>()
            .HasMany(u => u.Reviews)
            .WithOne(r => r.User)
            .HasForeignKey(r => r.UserId);
        
        modelBuilder.Entity<Borrow>()
            .HasMany(b => b.Reviews)
            .WithOne(r => r.Borrow)
            .HasForeignKey(r => r.BorrowId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Tool>()
            .HasMany(t => t.Schedules)
            .WithOne(s => s.Tool)
            .HasForeignKey(s => s.ToolsId);

        modelBuilder.Entity<Tool>()
            .HasMany(t => t.Accessories)
            .WithOne(ta => ta.Tool)
            .HasForeignKey(ta => ta.ToolId);

        modelBuilder.Entity<Tool>()
            .HasMany(t => t.Histories)
            .WithOne(h => h.Tool)
            .HasForeignKey(h => h.ToolId);

        modelBuilder.Entity<Tool>()
            .HasMany(t => t.SecurityDeposits)
            .WithOne(sd => sd.Tool)
            .HasForeignKey(sd => sd.ToolsId);

        modelBuilder.Entity<Tool>()
            .HasMany(t => t.Reviews)
            .WithOne(r => r.Tool)
            .HasForeignKey(r => r.ToolId);

        modelBuilder.Entity<Return>()
            .HasOne(r => r.Borrow)
            .WithMany(b => b.Returns)
            .HasForeignKey(r => r.BorrowsId);
        
        modelBuilder.Entity<Review>(e =>
        {
            e.Property(x => x.Status).HasDefaultValue("Pending");
            e.HasIndex(x => new { x.BorrowId, x.ToolId, x.UserId }).IsUnique();
        });
        modelBuilder.Entity<Category>().HasKey(u => u.Id);
        modelBuilder.Entity<Event>().HasKey(u => u.Id);
        modelBuilder.Entity<History>().HasKey(u => u.Id);
        modelBuilder.Entity<LendingPartner>().HasKey(u => u.Id);
        modelBuilder.Entity<Notification>().HasKey(u => u.Id);
        modelBuilder.Entity<Payment>().HasKey(u => u.Id);
        modelBuilder.Entity<ProductBorrow>().HasKey(u => u.Id);
        modelBuilder.Entity<Return>().HasKey(u => u.Id);
        modelBuilder.Entity<Schedule>().HasKey(u => u.Id);
        modelBuilder.Entity<SecurityDeposit>().HasKey(u => u.Id);
        modelBuilder.Entity<Tool>().HasKey(u => u.Id);
        modelBuilder.Entity<ToolAccessory>().HasKey(u => u.Id);
        modelBuilder.Entity<User>().HasKey(u => u.Id);
        
        
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username).IsUnique();
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email).IsUnique();
        
        modelBuilder.Entity<Borrow>(e =>
        {
            e.Property(x => x.Status).HasDefaultValue("Pending");
            e.Property(x => x.Date).HasDefaultValueSql("timezone('utc', now())");
            e.Property(x => x.Price).HasDefaultValue(0);
        });
        modelBuilder.Entity<ProductBorrow>(e =>
        {
            e.Property(x => x.Quantity).HasDefaultValue(1);
        });
    }

    public ToolLendingContext(DbContextOptions<ToolLendingContext> options) : base(options) { }
    
}

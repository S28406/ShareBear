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
        // modelBuilder.Entity<ProductBorrow>()
        //     .HasKey(pb => new { Tool_ID = pb.Tools_ID, Order_ID = pb.Orders_ID });
        //
        // modelBuilder.Entity<LendingPartner>()
        //     .HasKey(lp => new { User_Id = lp.Users_Id, Partner_Id = lp.Partners_Id });
        
        modelBuilder.Entity<Payment>(e =>
        {
            e.HasOne(p => p.Borrow)
                .WithMany(b => b.Payments)
                .HasForeignKey(p => p.Orders_ID)         
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<User>()
            .HasMany(u => u.Tools)
            .WithOne(t => t.User)
            .HasForeignKey(t => t.Users_ID);

        modelBuilder.Entity<User>()
            .HasMany(u => u.Borrows)
            .WithOne(b => b.User)
            .HasForeignKey(b => b.Users_ID);

        modelBuilder.Entity<Tool>()
            .HasMany(t => t.Schedules)
            .WithOne(s => s.Tool)
            .HasForeignKey(s => s.Tools_ID);

        modelBuilder.Entity<Tool>()
            .HasMany(t => t.Accessories)
            .WithOne(ta => ta.Tool)
            .HasForeignKey(ta => ta.Tool_ID);

        modelBuilder.Entity<Tool>()
            .HasMany(t => t.Histories)
            .WithOne(h => h.Tool)
            .HasForeignKey(h => h.Tool_ID);

        modelBuilder.Entity<Tool>()
            .HasMany(t => t.SecurityDeposits)
            .WithOne(sd => sd.Tool)
            .HasForeignKey(sd => sd.Tools_ID);

        modelBuilder.Entity<Tool>()
            .HasMany(t => t.Reviews)
            .WithOne(r => r.Tool)
            .HasForeignKey(r => r.ToolID);

        modelBuilder.Entity<Return>()
            .HasOne(r => r.Borrow)
            .WithMany(b => b.Returns)
            .HasForeignKey(r => r.Borrows_ID);
        
        
        modelBuilder.Entity<Borrow>().HasKey(u => u.ID);
        modelBuilder.Entity<Category>().HasKey(u => u.ID);
        modelBuilder.Entity<Event>().HasKey(u => u.ID);
        modelBuilder.Entity<History>().HasKey(u => u.ID);
        modelBuilder.Entity<LendingPartner>().HasKey(u => u.ID);
        modelBuilder.Entity<Notification>().HasKey(u => u.ID);
        modelBuilder.Entity<Payment>().HasKey(u => u.ID);
        modelBuilder.Entity<ProductBorrow>().HasKey(u => u.ID);
        modelBuilder.Entity<Return>().HasKey(u => u.ID);
        modelBuilder.Entity<Review>().HasKey(u => u.ID);
        modelBuilder.Entity<Schedule>().HasKey(u => u.ID);
        modelBuilder.Entity<SecurityDeposit>().HasKey(u => u.ID);
        modelBuilder.Entity<Tool>().HasKey(u => u.ID);
        modelBuilder.Entity<ToolAccessory>().HasKey(u => u.ID);
        modelBuilder.Entity<User>().HasKey(u => u.ID);
        
        
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

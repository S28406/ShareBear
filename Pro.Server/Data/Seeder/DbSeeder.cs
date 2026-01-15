using PRO.Data.Context;
using PRO.Models;
using Pro.Shared.Dtos;
using ToolRent.Security;

namespace PRO_.Data.Seeder;

public static class DbSeeder
{
    public static void Clean(ToolLendingContext context)
    {
        context.Borrows.RemoveRange(context.Borrows);
        context.Categories.RemoveRange(context.Categories);
        context.Events.RemoveRange(context.Events);
        context.Histories.RemoveRange(context.Histories);
        context.LendingPartners.RemoveRange(context.LendingPartners);
        context.Notifications.RemoveRange(context.Notifications);
        context.Payments.RemoveRange(context.Payments);
        context.ProductBorrows.RemoveRange(context.ProductBorrows);
        context.Returns.RemoveRange(context.Returns);
        context.Reviews.RemoveRange(context.Reviews);
        context.Schedules.RemoveRange(context.Schedules);
        context.SecurityDeposits.RemoveRange(context.SecurityDeposits);
        context.Tools.RemoveRange(context.Tools);
        context.ToolAccessories.RemoveRange(context.ToolAccessories);
        context.Users.RemoveRange(context.Users);

        context.SaveChanges();
        context.ChangeTracker.Clear();
    }

    public static void Seed(ToolLendingContext context)
    {
        Clean(context);

        // USERS
        var (adminHash, adminSalt) = PasswordHelper.Hash("admin123");
        var (johnHash, johnSalt) = PasswordHelper.Hash("password");

        var users = new List<User>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Username = "admin",
                Email = "admin@example.com",
                PasswordHash = adminHash,
                PasswordSalt = adminSalt,
                Role = "Admin"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Username = "john",
                Email = "john@example.com",
                PasswordHash = johnHash,
                PasswordSalt = johnSalt,
                Role = "Customer"
            }
        };
        context.Users.AddRange(users);
        context.SaveChanges();

        // CATEGORIES
        var categories = new List<Category>
        {
            new() { Id = Guid.NewGuid(), Name = "Power Tools", Description = "Electric or battery-powered tools" },
            new() { Id = Guid.NewGuid(), Name = "Hand Tools",  Description = "Manual tools for general use" },
            new() { Id = Guid.NewGuid(), Name = "Stations",    Description = "Big stations for industrial use" }
        };
        context.Categories.AddRange(categories);
        context.SaveChanges();

        // TOOLS
        var tools = new List<Tool>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Cordless Drill",
                Description = "18V cordless power drill",
                Price = 49.99f,
                Quantity = 10,
                UsersId = users[0].Id,
                CategoryId = categories[0].Id,
                ImagePath = "Drill.jpg",
                Location = "Warsaw"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Hammer",
                Description = "Heavy duty hammer",
                Price = 9.99f,
                Quantity = 20,
                UsersId = users[1].Id,
                CategoryId = categories[1].Id,
                ImagePath = "hamer.jpg",
                Location = "Krakow"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Band saw machine",
                Description = "Industrial band saw station",
                Price = 100f,
                Quantity = 1,
                UsersId = users[1].Id,
                CategoryId = categories[2].Id,
                ImagePath = "band_saw_machine.jpg",
                Location = "Gdansk"
            }
        };
        context.Tools.AddRange(tools);
        context.SaveChanges();
        
        
        var from = DateTime.UtcNow.Date;
        var to = from.AddMonths(6);

        context.Schedules.AddRange(tools.Select(t => new Schedule
        {
            Id = Guid.NewGuid(),
            ToolsId = t.Id,
            AvailableFrom = from,
            AvailableTo = to
        }));

        context.SaveChanges();
        
        
        // ACCESSORIES
        var accessories = new List<ToolAccessory>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Drill Bits",
                Description = "Assorted drill bits",
                QuantityAvailable = 50,
                ToolId = tools[0].Id
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Nails",
                Description = "Box of 100 nails",
                QuantityAvailable = 100,
                ToolId = tools[1].Id
            }
        };
        context.ToolAccessories.AddRange(accessories);
        context.SaveChanges();

        // HISTORY
        context.Histories.Add(new History
        {
            Id = Guid.NewGuid(),
            UserId = users[1].Id,
            ToolId = tools[0].Id,
            AddedAt = DateTime.UtcNow
        });
        context.SaveChanges();

        // REVIEWS
        var john = users[1];
        var reviewedTool = tools[0];

        var borrow1 = new Borrow
        {
            Id = Guid.NewGuid(),
            UsersId = john.Id,
            Status = "Paid",
            Date = DateTime.UtcNow.AddDays(-10),
            StartDate = DateTime.UtcNow.AddDays(-5).Date,
            EndDate = DateTime.UtcNow.AddDays(-4).Date,
            Price = reviewedTool.Price * 1 * 1
        };
        context.Borrows.Add(borrow1);

        context.ProductBorrows.Add(new ProductBorrow
        {
            Id = Guid.NewGuid(),
            BorrowId = borrow1.Id,
            ToolId = reviewedTool.Id,
            Quantity = 1
        });

        context.Reviews.Add(new Review
        {
            Id = Guid.NewGuid(),
            Rating = 5,
            Description = "Great product",
            Date = DateTime.UtcNow.AddDays(-3),
            ToolId = reviewedTool.Id,
            UserId = john.Id,
            BorrowId = borrow1.Id,
            Status = "Approved"
        });

        var borrow2 = new Borrow
        {
            Id = Guid.NewGuid(),
            UsersId = john.Id,
            Status = "Paid",
            Date = DateTime.UtcNow.AddDays(-20),
            StartDate = DateTime.UtcNow.AddDays(-15).Date,
            EndDate = DateTime.UtcNow.AddDays(-14).Date,
            Price = reviewedTool.Price * 1 * 1
        };
        context.Borrows.Add(borrow2);

        context.ProductBorrows.Add(new ProductBorrow
        {
            Id = Guid.NewGuid(),
            BorrowId = borrow2.Id,
            ToolId = reviewedTool.Id,
            Quantity = 1
        });

        // context.Reviews.Add(new Review
        // {
        //     Id = Guid.NewGuid(),
        //     Rating = 3,
        //     Description = "Okay",
        //     Date = DateTime.UtcNow.AddDays(-13),
        //     ToolId = reviewedTool.Id,
        //     UserId = john.Id,
        //     BorrowId = borrow2.Id,
        //     Status = "Approved"
        // });

        
        //Returns
        var borrowReturned = new Borrow
        {
            Id = Guid.NewGuid(),
            UsersId = john.Id,
            Status = BorrowStatuses.Returned,
            Date = DateTime.UtcNow.AddDays(-8),
            StartDate = DateTime.UtcNow.AddDays(-7).Date,
            EndDate = DateTime.UtcNow.AddDays(-6).Date,
            Price = reviewedTool.Price
        };
        context.Borrows.Add(borrowReturned);

        context.ProductBorrows.Add(new ProductBorrow
        {
            Id = Guid.NewGuid(),
            BorrowId = borrowReturned.Id,
            ToolId = reviewedTool.Id,
            Quantity = 1
        });

        context.Returns.Add(new Return
        {
            Id = Guid.NewGuid(),
            BorrowsId = borrowReturned.Id,
            Date = DateTime.UtcNow.AddDays(-6),
            Condition = "Clean, working.",
            Damage = "None"
        });

        context.SecurityDeposits.Add(new SecurityDeposit
        {
            Id = Guid.NewGuid(),
            ToolsId = reviewedTool.Id,
            UsersId = john.Id,
            Ammount = 50f,
            Status = DepositStatuses.Held,
            RefundDate = DateTime.UtcNow.AddDays(-6)
        });
        context.SaveChanges();
    }
}

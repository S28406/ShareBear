using PRO.Data.Context;
using PRO.Models;
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
        var (johnHash,  johnSalt ) = PasswordHelper.Hash("password");
        var users = new List<User>
        {
            new() { ID = Guid.NewGuid(), Username = "admin", Email = "admin@example.com", PasswordHash = adminHash,
                PasswordSalt = adminSalt, Role = "Admin" },
            new() { ID = Guid.NewGuid(), Username = "john",  Email = "john@example.com",  PasswordHash = adminHash,
                PasswordSalt = adminSalt,  Role = "Customer" }
        };
        context.Users.AddRange(users);
        context.SaveChanges();

        // CATEGORIES
        var categories = new List<Category>
        {
            new() { ID = Guid.NewGuid(), Name = "Power Tools", Description = "Electric or battery-powered tools" },
            new() { ID = Guid.NewGuid(), Name = "Hand Tools",  Description = "Manual tools for general use" },
            new() { ID = Guid.NewGuid(), Name = "Stations",  Description = "Big stations for industrial use" }
            
        };
        context.Categories.AddRange(categories);
        context.SaveChanges();

        // TOOLS
        var tools = new List<Tool>
        {
            new()
            {
                ID = Guid.NewGuid(),
                Name = "Cordless Drill", Description = "18V cordless power drill",
                Price = 49.99f, Quantity = 10, Users_ID = users[0].ID, CategoryId = categories[0].ID,
                ImagePath = "Drill.jpg"
            },
            new()
            {
                ID = Guid.NewGuid(),
                Name = "Hammer", Description = "Heavy duty hammer",
                Price = 9.99f, Quantity = 20, Users_ID = users[1].ID, CategoryId = categories[1].ID,
                ImagePath = "hamer.jpg"
            },
            new()
            {
                ID = Guid.NewGuid(),
                Name = "Band saw machine", Description = " ",
                Price = 100f, Quantity = 1, Users_ID = users[1].ID, CategoryId = categories[2].ID,
                ImagePath = "band_saw_machine.jpg"
            }
        };
        context.Tools.AddRange(tools);
        context.SaveChanges();

        // ACCESSORIES
        var accessories = new List<ToolAccessory>
        {
            new() { ID = Guid.NewGuid(), Name = "Drill Bits", Description = "Assorted drill bits", Quantity_Available = 50, Tool_ID = tools[0].ID },
            new() { ID = Guid.NewGuid(), Name = "Nails",      Description = "Box of 100 nails",      Quantity_Available = 100, Tool_ID = tools[1].ID }
        };
        context.ToolAccessories.AddRange(accessories);
        context.SaveChanges();

        // HISTORY
        context.Histories.Add(new History
        {
            ID = Guid.NewGuid(),
            UserID = users[1].ID,
            Tool_ID = tools[0].ID,
            Added_at = DateTime.UtcNow
        });
        context.SaveChanges();
    }
}

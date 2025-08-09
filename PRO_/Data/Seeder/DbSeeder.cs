﻿using PRO.Data.Context;
using PRO.Models;

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
    }
    public static void Seed(ToolLendingContext context)
    {
        // Clean(context);
        if (context.Users.Any()) return;

        
        // USERS
        var users = new List<User>
        {
            new User { ID = Guid.NewGuid(), Username = "admin", Email = "admin@example.com", Password = "admin123", Role = "Admin" },
            new User { ID = Guid.NewGuid(), Username = "john", Email = "john@example.com", Password = "password", Role = "Customer" }
        };
        context.Users.AddRange(users);
        context.SaveChanges();

        // CATEGORIES
        var categories = new List<Category>
        {
            new Category { ID = Guid.NewGuid(), Name = "Power Tools", Description = "Electric or battery-powered tools" },
            new Category { ID = Guid.NewGuid(), Name = "Hand Tools", Description = "Manual tools for general use" }
        };
        context.Categories.AddRange(categories);
        context.SaveChanges();

        // TOOLS
        var tools = new List<Tool>
        {
            new Tool {
                ID = Guid.NewGuid(), 
                Name = "Cordless Drill", Description = "18V cordless power drill",
                Price = 49.99f, Quantity = 10, Users_ID = users[0].ID, CategoryId = categories[0].ID,
                ImmagePath = "D:\\Projects\\PRO_\\PRO_\\Immages\\Drill.jpg"
            },
            new Tool {
                ID = Guid.NewGuid(), 
                Name = "Hammer", Description = "Heavy duty hammer",
                Price = 9.99f, Quantity = 20, Users_ID = users[1].ID, CategoryId = categories[1].ID,
                ImmagePath = "D:\\Projects\\PRO_\\PRO_\\Immages\\hamer.jpg"
            }
        };
        context.Tools.AddRange(tools);
        context.SaveChanges();

        // TOOL ACCESSORIES
        var accessories = new List<ToolAccessory>
        {
            new ToolAccessory { ID = Guid.NewGuid(), Name = "Drill Bits", Description = "Assorted drill bits", Quantity_Available = 50, Tool_ID = tools[0].ID },
            new ToolAccessory { ID = Guid.NewGuid(), Name = "Nails", Description = "Box of 100 nails", Quantity_Available = 100, Tool_ID = tools[1].ID }
        };
        context.ToolAccessories.AddRange(accessories);
        context.SaveChanges();

        // HISTORY
        var history = new History
        {
            ID = Guid.NewGuid(), 
            Users_ID = users[1].ID,
            Tool_ID = tools[0].ID,
            Added_at = DateTime.Now
        };
        context.Histories.Add(history);
        context.SaveChanges();
    }
}

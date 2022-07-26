using Microsoft.EntityFrameworkCore;

namespace ChangeRequestManagement.Entities.Seeds
{
    public static class SeedData
    {
        public static void SeedAllData(this ModelBuilder builder)
        {
            builder.Entity<Role>()
                .HasData(new Role
                {
                    RoleId = 1,
                    RoleName = "Admin".ToUpper(),
                });

            builder.Entity<Role>()
                .HasData(new Role
                {
                    RoleId = 2,
                    RoleName = "Client".ToUpper(),
                });

            builder.Entity<Person>()
                .HasData(new Person
                {
                    PersonId = 1,
                    FirstName = "Admin",
                    LastName = "Admin",
                    EmailAddress = "admin123@email.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                    RoleId = 1
                });

            builder.Entity<Person>()
                .HasData(new Person
                {
                    PersonId = 789,
                    FirstName = "Anuj",
                    LastName = "Tamrakar",
                    EmailAddress = "anuj.tamrakar@ekbana.info",
                    Password = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                    RoleId = 1
                });

            builder.Entity<Admin>()
                .HasData(new Admin
                {
                    AdminId = 2,
                    PersonId = 789
                });

            builder.Entity<Person>()
               .HasData(new Person
               {
                   PersonId = 2,
                   FirstName = "User",
                   LastName = "User",
                   EmailAddress = "user123@email.com",
                   Password = BCrypt.Net.BCrypt.HashPassword("User@123"),
                   RoleId = 2
               });

            builder.Entity<Admin>()
                .HasData(new Admin
                {
                    AdminId = 1,
                    PersonId = 1
                });

            builder.Entity<Company>()
                .HasData(new Company
                {
                    CompanyId = 1,
                    CompanyName = "ABC".ToUpper()
                });

            builder.Entity<Company>()
                .HasData(new Company
                {
                    CompanyId = 2,
                    CompanyName = "XYZ".ToUpper()
                });

            builder.Entity<Client>()
                .HasData(new Client
                {
                    ClientId = 1,
                    PersonId = 2,
                    CompanyId = 1
                });

            builder.Entity<ListItemCategory>()
                .HasData(new ListItemCategory
                {
                    ListItemCategoryId = 1,
                    ListItemCategoryName = "ChangeRequestType".ToUpper()
                });

            builder.Entity<ListItemCategory>()
               .HasData(new ListItemCategory
               {
                   ListItemCategoryId = 2,
                   ListItemCategoryName = "Priority".ToUpper()
               });

            builder.Entity<ListItemCategory>()
               .HasData(new ListItemCategory
               {
                   ListItemCategoryId = 3,
                   ListItemCategoryName = "ChangeRequestStatus".ToUpper()
               });

            builder.Entity<ListItem>()
               .HasData(new ListItem
               {
                   ListItemId = 1,
                   ListItemCategoryId = 1,
                   ListItemName = "Enhancement",
                   ListItemSystemName = "Enhancement".Trim().ToUpper()
               });

            builder.Entity<ListItem>()
              .HasData(new ListItem
              {
                  ListItemId = 2,
                  ListItemCategoryId = 1,
                  ListItemName = "Defect",
                  ListItemSystemName = "Defect".Trim().ToUpper()
              });

            builder.Entity<ListItem>()
              .HasData(new ListItem
              {
                  ListItemId = 3,
                  ListItemCategoryId = 2,
                  ListItemName = "Low",
                  ListItemSystemName = "Low".Trim().ToUpper()
              });

            builder.Entity<ListItem>()
             .HasData(new ListItem
             {
                 ListItemId = 4,
                 ListItemCategoryId = 2,
                 ListItemName = "Medium",
                 ListItemSystemName = "Medium".Trim().ToUpper()
             });

            builder.Entity<ListItem>()
             .HasData(new ListItem
             {
                 ListItemId = 5,
                 ListItemCategoryId = 2,
                 ListItemName = "High",
                 ListItemSystemName = "High".Trim().ToUpper()
             });

            builder.Entity<ListItem>()
             .HasData(new ListItem
             {
                 ListItemId = 6,
                 ListItemCategoryId = 2,
                 ListItemName = "Critical",
                 ListItemSystemName = "Critical".Trim().ToUpper()
             });

            builder.Entity<ListItem>()
             .HasData(new ListItem
             {
                 ListItemId = 7,
                 ListItemCategoryId = 3,
                 ListItemName = "Pending",
                 ListItemSystemName = "Pending".Trim().ToUpper()
             });

            builder.Entity<ListItem>()
             .HasData(new ListItem
             {
                 ListItemId = 8,
                 ListItemCategoryId = 3,
                 ListItemName = "Approved",
                 ListItemSystemName = "Approved".Trim().ToUpper()
             });

            builder.Entity<ListItem>()
             .HasData(new ListItem
             {
                 ListItemId = 9,
                 ListItemCategoryId = 3,
                 ListItemName = "Development",
                 ListItemSystemName = "Development".Trim().ToUpper()
             });

            builder.Entity<ListItem>()
             .HasData(new ListItem
             {
                 ListItemId = 10,
                 ListItemCategoryId = 3,
                 ListItemName = "Testing",
                 ListItemSystemName = "Testing".Trim().ToUpper()
             });

            builder.Entity<ListItem>()
             .HasData(new ListItem
             {
                 ListItemId = 11,
                 ListItemCategoryId = 3,
                 ListItemName = "Staging",
                 ListItemSystemName = "Staging".Trim().ToUpper()
             });

            builder.Entity<ListItem>()
             .HasData(new ListItem
             {
                 ListItemId = 12,
                 ListItemCategoryId = 3,
                 ListItemName = "Delivered",
                 ListItemSystemName = "Delivered".Trim().ToUpper()
             });
        }
    }
}
using Food.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Food.Core.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var admin = new ApplicationUser
            {
                Id = "admin",
                UserName = "Admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@prog.be",
                NormalizedEmail = "ADMIN@PROG.BE",
                FirstName = "Admin",
                LastName = "Admin",
                ConcurrencyStamp = Guid.NewGuid().ToString() 
            };

            var johnDoe = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "JohnDoe",
                NormalizedUserName = "JOHNDOE",
                Email = "john@prog.be",
                NormalizedEmail = "JOHN@PROG.BE",
                FirstName = "John",
                LastName = "Doe",
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };

            var passwordHasher = new PasswordHasher<ApplicationUser>();

            admin.PasswordHash = passwordHasher.HashPassword(admin, "Test123?");
            johnDoe.PasswordHash = passwordHasher.HashPassword(johnDoe, "Test123?");

            modelBuilder.Entity<ApplicationUser>().HasData(admin, johnDoe);           

            modelBuilder.Entity<Category>().HasData(
               new[]
               {
                    new Category
                    {
                        Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                        Name = "Pizza",
                        CreatedOn = DateTime.UtcNow,
                        LastEditedOn = DateTime.UtcNow
                    }, new Category
                    {
                        Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                        Name = "Pasta",
                        CreatedOn = DateTime.UtcNow,
                        LastEditedOn = DateTime.UtcNow
                    }, new Category
                    {
                        Id = Guid.Parse("00000000-0000-0000-0000-000000000003"),
                        Name = "Groenten",
                        CreatedOn = DateTime.UtcNow,
                        LastEditedOn = DateTime.UtcNow
                    }, new Category
                    {
                        Id = Guid.Parse("00000000-0000-0000-0000-000000000004"),
                        Name = "Fruit",
                        CreatedOn = DateTime.UtcNow,
                        LastEditedOn = DateTime.UtcNow
                    }
               }
           );

            modelBuilder.Entity<Product>().HasData(
               new[]
               {
                    new Product
                    {
                        Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                        Name = "Peperoni",
                        CategoryId = Guid.Parse("00000000-0000-0000-0000-000000000001"), // Pizza
                        CreatedOn = DateTime.UtcNow,
                        LastEditedOn = DateTime.UtcNow,
                        Image = "food/default.jpg"
                    }, new Product
                    {
                        Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                        Name = "Hawai",
                        CategoryId = Guid.Parse("00000000-0000-0000-0000-000000000001"), // Pizza
                        CreatedOn = DateTime.UtcNow,
                        LastEditedOn = DateTime.UtcNow,
                        Image = "food/default.jpg"
                    }, new Product
                    {
                        Id = Guid.Parse("00000000-0000-0000-0000-000000000003"),
                        Name = "Macaroni",
                        CategoryId = Guid.Parse("00000000-0000-0000-0000-000000000002"), // Pasta
                        CreatedOn = DateTime.UtcNow,
                        LastEditedOn = DateTime.UtcNow,
                        Image = "food/default.jpg"
                    }, new Product
                    {
                        Id = Guid.Parse("00000000-0000-0000-0000-000000000004"),
                        Name = "Spaghetti",
                        CategoryId = Guid.Parse("00000000-0000-0000-0000-000000000002"), // Pasta
                        CreatedOn = DateTime.UtcNow,
                        LastEditedOn = DateTime.UtcNow,
                        Image = "food/default.jpg"
                    }, new Product
                    {
                        Id = Guid.Parse("00000000-0000-0000-0000-000000000005"),
                        Name = "Komkommer",
                        CategoryId = Guid.Parse("00000000-0000-0000-0000-000000000003"), // Groenten
                        CreatedOn = DateTime.UtcNow,
                        LastEditedOn = DateTime.UtcNow,
                        Image = "food/default.jpg"
                    }, new Product
                    {
                        Id = Guid.Parse("00000000-0000-0000-0000-000000000006"),
                        Name = "Tomaat",
                        CategoryId = Guid.Parse("00000000-0000-0000-0000-000000000003"), // Groenten
                        CreatedOn = DateTime.UtcNow,
                        LastEditedOn = DateTime.UtcNow,
                        Image = "food/default.jpg"
                    }, new Product
                    {
                        Id = Guid.Parse("00000000-0000-0000-0000-000000000007"),
                        Name = "Appel",
                        CategoryId = Guid.Parse("00000000-0000-0000-0000-000000000004"), // Fruit
                        CreatedOn = DateTime.UtcNow,
                        LastEditedOn = DateTime.UtcNow,
                        Image = "food/default.jpg"
                    }, new Product
                    {
                        Id = Guid.Parse("00000000-0000-0000-0000-000000000008"),
                        Name = "Peer",
                        CategoryId = Guid.Parse("00000000-0000-0000-0000-000000000004"), // Fruit
                        CreatedOn = DateTime.UtcNow,
                        LastEditedOn = DateTime.UtcNow,
                        Image = "food/default.jpg"
                    }
               }
           );
        }
    }
}

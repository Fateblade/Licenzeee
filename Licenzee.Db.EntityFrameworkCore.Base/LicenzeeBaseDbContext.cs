using Fateblade.Licenzee.Db.Models;
using Microsoft.EntityFrameworkCore;

namespace Licenzee.Db.EntityFrameworkCore.Base
{
    public abstract class LicenzeeBaseDbContext : DbContext
    {
        public IQueryable<License> Licenses => LicenseSet.AsQueryable();
        public IQueryable<Product> Products => ProductSet.AsQueryable();
        public IQueryable<User> Users => UserSet.AsQueryable();
        public IQueryable<XLicenseUser> XLicenseUsers => XLicenseUserSet.AsQueryable();

        public DbSet<License> LicenseSet { get; set; }
        public DbSet<Product> ProductSet { get; set; }
        public DbSet<User> UserSet { get; set; }
        public DbSet<XLicenseUser> XLicenseUserSet { get; set; }


        protected LicenzeeBaseDbContext(bool ensureCreated = true)
        {
            if (ensureCreated)
            {// ReSharper disable once VirtualMemberCallInConstructor
                Database.EnsureCreated();
            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<License>().Property(t => t.Id).IsRequired();
            modelBuilder.Entity<License>().Property(t => t.Key).IsRequired();
            modelBuilder.Entity<License>().Property(t => t.ProductId).IsRequired();
            modelBuilder.Entity<License>().Property(t => t.UsageType).IsRequired();

            modelBuilder.Entity<Product>().Property(t => t.Name).IsRequired();
            modelBuilder.Entity<Product>().Property(t => t.Id).IsRequired();

            modelBuilder.Entity<User>().Property(t => t.Name).IsRequired();
            modelBuilder.Entity<User>().Property(t => t.Id).IsRequired();

            modelBuilder.Entity<XLicenseUser>().Property(t => t.LicenseId).IsRequired();
            modelBuilder.Entity<XLicenseUser>().Property(t => t.UserId).IsRequired();
            modelBuilder.Entity<XLicenseUser>().HasKey(t => new { t.LicenseId, t.UserId });
        }


        public License CreateLicense(string key, int productId, UsageType usageType,
            string usageComment, params User[] licenseUsers)
        {
            var createdLicenseEntry = LicenseSet.Add(new License
            {
                Key = key,
                ProductId = productId,
                UsageType = usageType,
                LicenseUserId = 0
            });

            switch (usageType)
            {
                case UsageType.Comment:
                    createdLicenseEntry.Entity.UsageComment = usageComment;
                    break;
                case UsageType.SingleUser:
                    createdLicenseEntry.Entity.LicenseUserId = licenseUsers[0].Id;
                    break;
                default:
                {
                    foreach (var t in licenseUsers)
                    {
                        XLicenseUserSet.Add(new XLicenseUser(createdLicenseEntry.Entity.Id, t.Id));
                    }

                    break;
                }
            }

            SaveChanges();

            return createdLicenseEntry.Entity;
        }

        public License UpdateLicense(int licenseId, string key, int productId, UsageType usageType, string usageComment,
            User[] users)
        {
            var license = Licenses.First(t => t.Id == licenseId);
            XLicenseUserSet.RemoveRange(XLicenseUsers.Where(t => t.LicenseId == licenseId));

            license.Key = key;
            license.ProductId = productId;

            switch (usageType)
            {
                case UsageType.Comment:
                    license.UsageComment = usageComment;
                    break;
                case UsageType.SingleUser:
                    license.LicenseUserId = users.Length > 0 ? users[0].Id : 0;
                    break;
                default:
                {
                    foreach (var t in users)
                    {
                        XLicenseUserSet.Add(new XLicenseUser(license.Id, t.Id));
                    }

                    break;
                }
            }

            LicenseSet.Update(license);
            SaveChanges();
            return license;
        }

        public void DeleteLicense(int licenseId)
        {
            LicenseSet.Remove(Licenses.First(t => t.Id == licenseId));
            XLicenseUserSet.RemoveRange(XLicenseUsers.Where(t => t.LicenseId == licenseId));

            SaveChanges();
        }

        public User[] GetUsersOfLicense(int licenseId)
        {
            return XLicenseUsers.Where(t => t.LicenseId == licenseId)
                .Select(x => Users.First(u => u.Id == x.UserId))
                .ToArray();
        }

        public Product CreateProduct(string name, string version, string licenser, string comment)
        {
            var createdProductEntry = ProductSet.Add(new Product
            {
                Name = name,
                Version = version,
                Comment = comment,
                Licenser = licenser,
            });

            SaveChanges();
            return createdProductEntry.Entity;
        }

        public Product UpdateProduct(int toModifyId, string name, string version, string licenser, string comment)
        {
            var product = Products.First(t => t.Id == toModifyId);

            product.Name = name;
            product.Version = version;
            product.Comment = comment;
            product.Licenser = licenser;

            ProductSet.Update(product);
            SaveChanges();

            return product;
        }

        public void DeleteProduct(int licensedProductId)
        {
            ProductSet.Remove(Products.First(t => t.Id == licensedProductId));
            LicenseSet.RemoveRange(Licenses.Where(t => t.ProductId == licensedProductId));

            SaveChanges();
        }

        public User CreateUser(string name, string comment)
        {
            var createdUser = UserSet.Add(new User { Comment = comment, Name = name });

            SaveChanges();
            return createdUser.Entity;
        }

        public User UpdateUser(int toModifyId, string name, string comment)
        {
            var user = Users.First(t => t.Id == toModifyId);

            user.Name = name;
            user.Comment = comment;
            UserSet.Update(user);

            SaveChanges();

            return user;
        }

        public void DeleteUser(int userId)
        {
            UserSet.Remove(Users.First(t => t.Id == userId));
            XLicenseUserSet.RemoveRange(XLicenseUsers.Where(t => t.UserId == userId));

            SaveChanges();
        }
    }
}
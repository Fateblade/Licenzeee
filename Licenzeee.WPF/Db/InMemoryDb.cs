using Fateblade.Licenzee.Db;
using Fateblade.Licenzee.Db.Models;
using System.Collections.Generic;
using System.Linq;
using License = Fateblade.Licenzee.Db.Models.License;

namespace Fateblade.Licenzeee.WPF.Db;

public class InMemoryDb : IDb
{
    private readonly List<License> _licenses = new();
    private readonly List<Product> _products = new();
    private readonly List<User> _users = new();
    private readonly List<XLicenseUser> _xLicenseUsers = new();


    public IQueryable<License> Licenses => _licenses.AsQueryable();
    public IQueryable<Product> Products => _products.AsQueryable();
    public IQueryable<User> Users => _users.AsQueryable();
    public IQueryable<XLicenseUser> XLicenseUsers => _xLicenseUsers.AsQueryable();


    public License CreateLicense(string key, int productId, UsageType usageType, string usageComment,
        params User[] licenseUsers)
    {
        var id = Licenses.Max(t => t.Id) + 1;

        var license = new License { Id = id, Key = key, ProductId = productId, UsageType = usageType };

        switch (usageType)
        {
            case UsageType.Comment:
                license.UsageComment = usageComment;
                break;
            case UsageType.SingleUser:
                license.LicenseUserId = licenseUsers[0].Id;
                break;
            default:
                {
                    foreach (var t in licenseUsers)
                    {
                        _xLicenseUsers.Add(new XLicenseUser(id, t.Id));
                    }

                    break;
                }
        }

        _licenses.Add(license);
        return license;
    }

    public License UpdateLicense(int licenseId, string key, int productId, UsageType usageType, string usageComment, User[] users)
    {
        var license = Licenses.First(t => t.Id == licenseId);
        _xLicenseUsers.RemoveAll(t => t.LicenseId == licenseId);

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
                        _xLicenseUsers.Add(new XLicenseUser(licenseId, t.Id));
                    }

                    break;
                }
        }

        return license;
    }

    public void DeleteLicense(int licenseId)
    {
        var indexToDelete = _licenses.FindIndex(t => t.Id == licenseId);
        if (indexToDelete >= 0)
        {
            _licenses.RemoveAt(indexToDelete);

            _xLicenseUsers.RemoveAll(t => t.LicenseId == licenseId);
        }
    }

    public User[] GetUsersOfLicense(int licenseId)
    {
        return XLicenseUsers.Where(t => t.LicenseId == licenseId)
            .Select(x => Users.First(u => u.Id == x.UserId))
            .ToArray();
    }

    public Product CreateProduct(string name, string version, string licenser, string comment)
    {
        var id = Products.Max(t => t.Id) + 1;

        var product = new Product { Id = id, Name = name, Version = version, Comment = comment, Licenser = licenser };
        _products.Add(product);

        return product;
    }

    public Product UpdateProduct(int toModifyId, string name, string version, string licenser, string comment)
    {
        var product = Products.First(t => t.Id == toModifyId);

        product.Name = name;
        product.Version = version;
        product.Comment = comment;
        product.Licenser = licenser;

        return product;
    }

    public void DeleteProduct(int licensedProductId)
    {
        var associatedLicenseIds = Licenses.Where(t => t.ProductId == licensedProductId).Select(t => t.Id).ToArray();

        var indexToDelete = _products.FindIndex(t => t.Id == licensedProductId);
        if (indexToDelete >= 0)
        {
            _products.RemoveAt(indexToDelete);
            foreach (var associatedLicenseId in associatedLicenseIds)
            {
                DeleteLicense(associatedLicenseId);
            }
        }
    }

    public User CreateUser(string name, string comment)
    {
        var id = Users.Max(t => t.Id) + 1;

        var user = new User { Id = id, Comment = comment };
        _users.Add(user);

        return user;
    }

    public User UpdateUser(int toModifyId, string name, string comment)
    {
        var user = Users.First(t => t.Id == toModifyId);

        user.Name = name;
        user.Comment = comment;

        return user;
    }

    public void DeleteUser(int userId)
    {
        var indexToDelete = _users.FindIndex(t => t.Id == userId);
        if (indexToDelete >= 0)
        {
            _users.RemoveAt(indexToDelete);
        }

        var associatedLicenses = Licenses.Where(t => t.LicenseUserId == userId);
        foreach (var associatedLicense in associatedLicenses)
        {
            associatedLicense.LicenseUserId = 0;
        }

        _xLicenseUsers.RemoveAll(t => t.UserId == userId);
    }

}
using Fateblade.Licenzee.Db;
using Fateblade.Licenzee.Db.Models;
using System.Collections.Generic;
using System.Linq;
namespace Fateblade.Licenzeee.WPF;

public class InMemoryDb : IDb
{
    private readonly List<License> _licenses = new();
    private readonly List<Product> _products = new();
    private readonly List<UsageType> _usageTypes = new(); 
    private readonly List<User> _users = new();
    private readonly List<XLicenseUser> _xLicenseUsers = new();


    public IQueryable<License> Licenses => _licenses.AsQueryable();
    public IQueryable<Product> Products => _products.AsQueryable();
    public IQueryable<UsageType> UsageTypes => _usageTypes.AsQueryable();
    public IQueryable<User> Users => _users.AsQueryable();
    public IQueryable<XLicenseUser> XLicenseUsers => _xLicenseUsers.AsQueryable();
    
    private static InMemoryDb? _instance;
    public static InMemoryDb Instance {
        get
        {
            if (_instance == null)
            {
                _instance = new InMemoryDb();
            }

            return _instance;
        }
    }
    

    private InMemoryDb()
    {
        _usageTypes.Add(new(1, "Comment"));
        _usageTypes.Add(new(2, "SingleUser"));
        _usageTypes.Add(new(3, "MultiUser"));

        //Debug Sample Data
        _licenses.Add(new License { Id = 1, Key = "Key1", ProductId = 1, UsageComment = "UsageComment1", UsageTypeId = 1, LicenseUserId = 0 });
        _licenses.Add(new License { Id = 2, Key = "Key2", ProductId = 2, UsageComment = "UsageComment2", UsageTypeId = 1, LicenseUserId = 0 });
        _licenses.Add(new License { Id = 3, Key = "Key3", ProductId = 1, UsageComment = "UsageComment3", UsageTypeId = 2, LicenseUserId = 1 });
        _licenses.Add(new License { Id = 4, Key = "Key4", ProductId = 3, UsageComment = "UsageComment4", UsageTypeId = 2, LicenseUserId = 2 });
        _licenses.Add(new License { Id = 5, Key = "Key5", ProductId = 1, UsageComment = "UsageComment5", UsageTypeId = 2, LicenseUserId = 3 });
        _licenses.Add(new License { Id = 6, Key = "Key6", ProductId = 4, UsageComment = "UsageComment6", UsageTypeId = 2, LicenseUserId = 4 });
        _licenses.Add(new License { Id = 7, Key = "Key7", ProductId = 1, UsageComment = "UsageComment7", UsageTypeId = 3, LicenseUserId = 0 });
        _licenses.Add(new License { Id = 8, Key = "Key8", ProductId = 2, UsageComment = "UsageComment8", UsageTypeId = 3, LicenseUserId = 0 });

        _products.Add(new Product { Id = 1, Licenser = "LicenseGiver1", Name = "ProductName1", Version = "Version1" });
        _products.Add(new Product { Id = 2, Licenser = "LicenseGiver1", Name = "ProductName2", Version = "Version2" });
        _products.Add(new Product { Id = 3, Licenser = "LicenseGiver2", Name = "ProductName3", Version = "Version3" });
        _products.Add(new Product { Id = 4, Licenser = "LicenseGiver2", Name = "ProductName4", Version = "Version4" });
        _products.Add(new Product { Id = 5, Licenser = "LicenseGiver2", Name = "ProductName3", Version = "Version3.5" });

        _users.Add(new User { Id = 1, Name = "LicenseUser1", Comment = "Comment1" });
        _users.Add(new User { Id = 2, Name = "LicenseUser2", Comment = "Comment2" });
        _users.Add(new User { Id = 3, Name = "LicenseUser3", Comment = "Comment3" });
        _users.Add(new User { Id = 4, Name = "LicenseUser4", Comment = "Comment4" });
        _users.Add(new User { Id = 5, Name = "LicenseUser5", Comment = "Comment5" });
        _users.Add(new User { Id = 6, Name = "LicenseUser6", Comment = "Comment6" });

        _xLicenseUsers.Add(new XLicenseUser(7, 1));
        _xLicenseUsers.Add(new XLicenseUser(7, 3));
        _xLicenseUsers.Add(new XLicenseUser(7, 5));
        _xLicenseUsers.Add(new XLicenseUser(8, 2));
        _xLicenseUsers.Add(new XLicenseUser(8, 4));
        _xLicenseUsers.Add(new XLicenseUser(8, 6));
    }


    public License CreateLicense(string key, int productId, int usageTypeId, string usageComment, params User[] licenseUsers)
    {
        var id = Licenses.Max(t => t.Id) + 1;

        var license = new License { Id = id, Key = key, ProductId = productId, UsageTypeId = usageTypeId };

        switch (usageTypeId)
        {
            case 1:
                license.UsageComment = usageComment;
                break;
            case 2:
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

    public License UpdateLicense(int licenseId, string key, int productId, int usageTypeId, string usageComment, User[] users)
    {
        var license = Licenses.First(t => t.Id == licenseId);
        _xLicenseUsers.RemoveAll(t => t.LicenseId == licenseId);

        license.Key = key;
        license.ProductId = productId;

        switch (usageTypeId)
        {
            case 1:
                license.UsageComment = usageComment;
                break;
            case 2:
                license.LicenseUserId = users[0].Id;
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
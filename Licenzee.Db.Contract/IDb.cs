using System.Linq;
using Fateblade.Licenzee.Db.Models;

namespace Fateblade.Licenzee.Db
{
    public interface IDb
    {
        IQueryable<License> Licenses { get; }
        IQueryable<Product> Products { get; }
        IQueryable<User> Users { get; }
        IQueryable<XLicenseUser> XLicenseUsers { get; }

        License CreateLicense(string key, int productId, UsageType usageType, string usageComment, params User[] licenseUsers);
        License UpdateLicense(int licenseId, string key, int productId, UsageType usageType, string usageComment, User[] users);
        void DeleteLicense(int licenseId);
        User[] GetUsersOfLicense(int licenseId);

        Product CreateProduct(string name, string version, string licenser, string comment);
        Product UpdateProduct(int toModifyId, string name, string version, string licenser, string comment);
        void DeleteProduct(int licensedProductId);

        User CreateUser(string name, string comment);
        User UpdateUser(int toModifyId, string name, string comment);
        void DeleteUser(int userId);
    }
}

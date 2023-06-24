using Fateblade.Licenzee.Db;
using Fateblade.Licenzee.Db.Models;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fateblade.Licenzeee.WPF.Db
{
    interface IDbMigrator
    {
        void Migrate(IDb oldDb, IDb newDb);
    }

    internal class DbMigrator : IDbMigrator
    {
        private readonly IEventAggregator _eventAggregator;


        public DbMigrator(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }


        public void Migrate(IDb oldDb, IDb newDb)
        {
            Dictionary<int, int> productIdMapping = new Dictionary<int, int>(oldDb.Products.Count());
            Dictionary<int, int> userIdMapping = new Dictionary<int, int>(oldDb.Users.Count());

            oldDb.Products.ToList().ForEach(
                product =>
                    productIdMapping[product.Id] = newDb.CreateProduct(
                        product.Name,
                        product.Version,
                        product.Licenser,
                        product.Comment).Id);

            oldDb.Users.ToList().ForEach(
                user =>
                    userIdMapping[user.Id] = newDb.CreateUser(
                        user.Name,
                        user.Comment).Id);

            foreach (var license in oldDb.Licenses)
            {
                newDb.CreateLicense(
                    license.Key,
                    productIdMapping[license.ProductId],
                    license.UsageType,
                    license.UsageComment,
                    license.UsageType == UsageType.SingleUser ? new[] { newDb.Users.First(t => t.Id == userIdMapping[license.LicenseUserId]) }
                    : license.UsageType == UsageType.MultiUser ? oldDb.XLicenseUsers
                        .Where(t => t.LicenseId == license.Id)
                        .Select(t => userIdMapping[t.UserId])
                        .Select(t => newDb.Users.First(x => x.Id == t))
                        .ToArray()
                    : Array.Empty<User>()
                );
            }
        }
    }
}

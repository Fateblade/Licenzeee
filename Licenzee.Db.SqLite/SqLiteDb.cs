using Licenzee.Db.EntityFrameworkCore.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

namespace Fateblade.Licenzee.Db.SqLite
{
    public class SqLiteDb : LicenzeeBaseDbContext, IDb
    {
        private readonly string _completeDbFilePath;

        public SqLiteDb(string completeDbFilePath)
        {
            _completeDbFilePath = completeDbFilePath;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var directoryPath = Directory.GetParent(_completeDbFilePath)?.FullName ?? throw new ArgumentException($"'{_completeDbFilePath}' is not in a valid path");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            optionsBuilder.UseSqlite($"Data Source={_completeDbFilePath};");
        }
    }
}

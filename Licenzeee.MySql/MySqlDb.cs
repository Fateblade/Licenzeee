using Fateblade.Licenzee.Db;
using Licenzee.Db.EntityFrameworkCore.Base;
using Microsoft.EntityFrameworkCore;

namespace Fateblade.Licenzeee.MySql;

public class MySqlDb : LicenzeeBaseDbContext, IDb
{
    private readonly string _serverName;
    private readonly string _databaseName;
    private readonly string _userName;
    private readonly string _userPassword;
        
    //TODO: password should probably not be passed in clear text?
    public MySqlDb(string serverName, string databaseName, string userName, string userPassword)
    {
        _serverName = serverName;
        _databaseName = databaseName;
        _userName = userName;
        _userPassword = userPassword;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySql($"Server={_serverName}; Database={_databaseName}; Uid={_userName}; Pwd={_userPassword}; SslMode=Required;", 
            MySqlServerVersion.LatestSupportedServerVersion);
    }
}
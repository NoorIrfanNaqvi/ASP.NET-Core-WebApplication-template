using Microsoft.EntityFrameworkCore;

namespace ASPlogInV2.Models
{
    public class DbConnectorSQLite : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite(@"data source=C:\SQLiteDb.db");
        //For the above, inside the options.UseSqlite() add this inside the brackets:
        //@"data source=[filepath]\SQLite.db"
        //Replace the '[filepath]' with the actual file location to store the database, or replace it with MS SQL instead.

        //Database tables go here vvv
        public DbSet<UserAccounts> UserAccounts { get; set; }
    }
}
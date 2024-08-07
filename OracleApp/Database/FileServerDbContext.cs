using Microsoft.EntityFrameworkCore;
using OracleApp.Database.Models;

namespace OracleApp.Database
{
    public class FileServerDbContext : DbContext
    {
        public FileServerDbContext(DbContextOptions<FileServerDbContext> options) : base(options)
        {
        }

        public DbSet<FileInformation> FileInfo { get; set; }
    }
}

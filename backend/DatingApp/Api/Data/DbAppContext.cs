using DatingApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Data
{
    public class DbAppContext : DbContext
    {
        public DbAppContext(DbContextOptions options) : base(options){ }
        public DbSet<AppUser> Users { get; set; }

    }
}

using Api.Entities;
using DatingApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Data
{
    public class DbAppContext : DbContext
    {
        public DbAppContext(DbContextOptions options) : base(options) { }
        public DbSet<AppUser> Users { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Photo> Photos { get; set; }
    }
}

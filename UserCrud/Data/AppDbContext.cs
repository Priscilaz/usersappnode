using Microsoft.EntityFrameworkCore;
using UserCrud.Models;

namespace UserCrud.Data
{
    public class AppDbContext : DbContext
    {

       // public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        //public DbSet<User> Users { get; set; }
    }
}

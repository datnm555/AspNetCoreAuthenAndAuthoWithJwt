using AspNetCoreAuthenAndAuthoWithJwt.Entities;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreAuthenAndAuthoWithJwt.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;
using SkipTake_Demo.Entities;

namespace SkipTake_Demo
{
    public class DogsDbContext : DbContext
    {
        public DogsDbContext(DbContextOptions<DogsDbContext> options) : base(options)
        {
        }

        public DbSet<Dog> Dogs { get; set; }
        public DbSet<Toy> Toys { get; set; }
    }
}

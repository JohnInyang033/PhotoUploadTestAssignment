using BandLabTestAssignment.Common.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace BandLabTestAssignment.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Post> Posts { get; set; } 

        public DbSet<Comment> Comments { get; set; }
    }
}

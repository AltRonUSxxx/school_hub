using Microsoft.EntityFrameworkCore;
using SchoolHub.Models;


namespace SchoolHub.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users {  get; set; }
        public DbSet<Project> Projects { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
        
        }
    }
}

using AdminPanel.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace AdminPanel.Database
{
    public class MyDbContext : DbContext
    {
        public DbSet<Album> Albums { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        
        public MyDbContext(DbContextOptions<MyDbContext> options): base(options)
        {
            Database.EnsureCreated();   
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}

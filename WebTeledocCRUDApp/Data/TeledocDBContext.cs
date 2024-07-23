using Microsoft.EntityFrameworkCore;
namespace WebTeledocCRUDApp.Data
{
    public class TeledocDBContext : DbContext
    {
        public TeledocDBContext(DbContextOptions<TeledocDBContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Enterprise> Enterprises { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(u =>
            {
                u.HasMany(u => u.Enterprises).
                WithMany(e => e.Users).
                UsingEntity(e => { e.ToTable("UserEnterprise"); });
            });
        }

    }
}
